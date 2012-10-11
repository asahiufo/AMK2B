from mathutils import Vector
from .osc.oscd import Method
from .osc.oscd import ThreadingServer
import threading


class KinectDataReceiver(object):

    def __init__(self):
        self._server = None
        self._thread = None
        self._users = dict()

        self._started = False

    def __del__(self):
        self.stop()

    def start(self):
        if self._started:
            return
        self._init_osc()
        self._started = True

    def stop(self):
        if not self._started:
            return
        self._thread.stop()
        self._server.shutdown()
        self._started = False

    def get_user(self, user_no):
        if not user_no in self._users.keys():
            return None
        return self._users[user_no]

    def _init_osc(self):
        self._server = ThreadingServer(("127.0.0.1", 38040))
        self._server.daemon_threads = True
        self._server.add_method(
            KinectDataReceiver.OSCCallback(
                address="/skeleton",
                function=self._receive_callback
            )
        )
        self._thread = threading.Thread(
            target=self._server.serve_forever,
            name="osc_frame_polling"
        )
        self._thread.setDaemon(True)
        self._thread.start()

    def _receive_callback(self, address, data):
        length = len(data)
        i = 0
        if length >= 5:
            user_no = data[0]
            size_proportion = float(data[1])
            center_x = float(data[2])
            center_y = float(data[3])
            center_z = float(data[4])
            i = i + 5
        else:
            return

        if not user_no in self._users.keys():
            self._users[user_no] = KinectUser(user_no)

        user = self._users[user_no]
        user.set_adjustment_value(
            size_proportion,
            [center_x, center_y, center_z]
        )
        user.reset_joints()

        while i < length:
            if length - i >= 4:
                joint_name = data[i + 0]
                position_x = float(data[i + 1])
                position_y = float(data[i + 2])
                position_z = float(data[i + 3])
                i = i + 4
                user.set_joint_location(
                    joint_name,
                    [position_x, position_y, position_z]
                )

    class OSCCallback(Method):

        def __init__(self, address, function):
            Method.__init__(self, address)
            self._function = function

        def __call__(self, address, typetags, data):
            self._function(address, data)


class KinectUser(object):

    def __init__(self, no):
        self._no = no

        self._size_proportion = 0
        self._center_position = None

        self._joints = None

    def set_adjustment_value(self, size_proportion, center_position):
        self._size_proportion = size_proportion
        self._center_position = Vector(center_position)

    def reset_joints(self):
        self._joints = dict()

    def set_joint_location(self, joint_name, position):
        if not joint_name in self._joints.keys():
            self._joints[joint_name] = KinectUser.Joint()

        joint = self._joints[joint_name]
        position_vec = Vector(position)
        joint.location.x = position_vec.x
        joint.location.y = position_vec.y
        joint.location.z = position_vec.z

    class Joint(object):

        def __init__(self):
            self.location = Vector((0, 0, 0))
