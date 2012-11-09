import bpy


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
            #context.window_manager.event_timer_remove(self._event)
            #context.window_manager.modal_handler_remove(self)
            bpy.amk2b.kinect_data_applying_started = False
        return {"FINISHED"}

    def modal(self, context, event):
        if event.type == "TIMER":
            if bpy.amk2b.kinect_data_applying_started:
                for user in bpy.amk2b.kinect_data_receiver.users.values():
                    bpy.amk2b.blender_data_manager.apply_location(user)
            return {"RUNNING_MODAL"}
        return {"PASS_THROUGH"}

    def invoke(self, context, event):
        self.execute(context)
        return {"RUNNING_MODAL"}


class RecordingOperator(bpy.types.Operator):

    bl_idname = "amk2b.recording_operator"
    bl_label = "Recording Start and Stop"

    @classmethod
    def poll(cls, context):
        return bpy.amk2b.kinect_data_applying_started

    def execute(self, context):
        if not bpy.amk2b.recording_pre_started:
            self._event = context.window_manager.event_timer_add(
                1 / 60,
                context.window
            )
            context.window_manager.modal_handler_add(self)
            bpy.amk2b.recording_wait_time = 500
            bpy.amk2b.recording_started = False
            bpy.amk2b.recording_pre_started = True
        else:
            #context.window_manager.event_timer_remove(self._event)
            #context.window_manager.modal_handler_remove(self)
            if context.screen.is_animation_playing:
                bpy.ops.screen.animation_play()
            bpy.amk2b.recording_pre_started = False
            bpy.amk2b.recording_started = False
        return {"FINISHED"}

    def modal(self, context, event):
        if context.area.type == 'VIEW_3D':
            context.area.tag_redraw()
        if event.type == "TIMER":
            if (bpy.amk2b.recording_pre_started and
                not bpy.amk2b.recording_started):
                bpy.amk2b.recording_wait_time -= 1
                if bpy.amk2b.recording_wait_time <= 0:
                    context.scene.frame_current = context.scene.frame_start
                    if not context.screen.is_animation_playing:
                        bpy.ops.screen.animation_play()
                    bpy.amk2b.recording_started = True
            else:
                if not bpy.amk2b.recording_started:
                    return {"FINISHED"}
                if context.scene.frame_current >= context.scene.frame_end:
                    self.execute(context)
            return {"RUNNING_MODAL"}
        return {"PASS_THROUGH"}

    def invoke(self, context, event):
        self.execute(context)
        return {"RUNNING_MODAL"}
