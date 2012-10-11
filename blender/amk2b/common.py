from .kinect import KinectDataReceiver


class ComObject(object):

    def __init__(self):
        self.kinect_data_receiving_started = False
        self.user_drawing_started = False
        self.kinect_data_applying_started = False
        self.kinect_data_receiver = KinectDataReceiver()
