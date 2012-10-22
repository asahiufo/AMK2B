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


class RecordingOperator(bpy.types.Operator):

    bl_idname = "amk2b.recording_operator"
    bl_label = "Recording Start and Stop"

    def __init__(self):
        self._waiting_time = 5
        self._start_time = 0

    @classmethod
    def poll(cls, context):
        return bpy.amk2b.kinect_data_receiving_started

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
