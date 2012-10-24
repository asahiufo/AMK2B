import bpy
import time


class KinectDataReceivingOperator(bpy.types.Operator):

    bl_idname = "amk2b.kinect_data_receiving_operator"
    bl_label = "Kinect Data Receiving Start and Stop"

    def execute(self, context):
        if not bpy.amk2b.kinect_data_receiving_started:
            bpy.amk2b.kinect_data_receiver.start()
            bpy.amk2b.kinect_data_receiving_started = True
        else:
            bpy.amk2b.kinect_data_receiver.stop()
            bpy.amk2b.kinect_data_receiving_started = False
        return {"FINISHED"}


class KinectDataApplyingOperator(bpy.types.Operator):

    bl_idname = "amk2b.kinect_data_applying_operator"
    bl_label = "Kinect Data Applying To Bone Start and Stop"

    @classmethod
    def poll(cls, context):
        return bpy.amk2b.kinect_data_receiving_started

    def execute(self, context):
        if not bpy.amk2b.kinect_data_applying_started:
            self._event = context.window_manager.event_timer_add(
                1 / 60,
                context.window
            )
            context.window_manager.modal_handler_add(self)
            bpy.amk2b.kinect_data_applying_started = True
        else:
            context.window_manager.event_timer_remove(self._event)
            context.window_manager.modal_handler_remove(self)
            bpy.amk2b.kinect_data_applying_started = False
        return {"FINISHED"}

    def modal(self, context, event):
        if event.type == "TIMER":
            if bpy.amk2b.kinect_data_applying_started:
                for user in bpy.amk2b.kinect_data_receiver.users.values():
                    bpy.amk2b.blender_data_manager.apply_location(user)
                    scene = bpy.context.scene
                    if scene.frame_current > (scene.frame_end - 1):
                        bpy.amk2b.recording_started = False
            return {"RUNNING_MODAL"}
        return {"PASS_THROUGH"}

    def invoke(self, context, event):
        self.execute(context)
        return {"RUNNING_MODAL"}


class RecordingOperator(bpy.types.Operator):

    bl_idname = "amk2b.recording_operator"
    bl_label = "Recording Start and Stop"

    def __init__(self):
        self._waiting_time = 5
        self._start_time = 0

    @classmethod
    def poll(cls, context):
        return bpy.amk2b.kinect_data_applying_started

    def execute(self, context):
        if not bpy.amk2b.recording_started:
            context.scene.frame_current = context.scene.frame_start
            if not context.screen.is_animation_playing:
                bpy.ops.screen.animation_play()
            bpy.amk2b.recording_started = True
        else:
            if context.screen.is_animation_playing:
                bpy.ops.screen.animation_play()
            bpy.amk2b.recording_started = False
        return {"FINISHED"}

    def modal(self, context, event):
        if bpy.amk2b.recording_started:
            return self.execute(context)

        if not event.type.startswith("TIMER"):
            return {"PASS_THROUGH"}

        t = time.time() - self._start_time
        if t < self._waiting_time:
            return {"PASS_THROUGH"}

        return self.execute(context)
        return {"PASS_THROUGH"}

    def invoke(self, context, event):
        if bpy.amk2b.recording_started:
            return self.execute(context)
        context.window_manager.modal_handler_add(self)
        self._start_time = time.time()
        return {"RUNNING_MODAL"}
