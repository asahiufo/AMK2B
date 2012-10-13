import bpy


class KinectDataReceivingOperator(bpy.types.Operator):

    bl_idname = "amk2b.kinect_data_receiving_operator"
    bl_label = "Kinect Data Receiving Start and Stop"

    def execute(self, context):
        if not bpy.amk2b.kinect_data_receiving_started:
            bpy.amk2b.blender_data_manager.set_target_object()
            bpy.amk2b.kinect_data_receiver.start()
            bpy.amk2b.kinect_data_receiving_started = True
        else:
            bpy.amk2b.kinect_data_receiver.stop()
            bpy.amk2b.blender_data_manager.unset_target_object()
            bpy.amk2b.kinect_data_receiving_started = False
        return {"FINISHED"}


class UserDrawingOperator(bpy.types.Operator):

    bl_idname = "amk2b.user_drawing_operator"
    bl_label = "Receive Start and Stop"

    def __init__(self):
        self._view_3d_area = None
        self._window_region = None
        self._user_drawing_callback_func = None

    def execute(self, context):
        if not bpy.amk2b.user_drawing_started:
            self._start_draw(context)
            bpy.amk2b.user_drawing_started = True
        else:
            self._stop_draw(context)
            bpy.amk2b.user_drawing_started = False
        return {"FINISHED"}

    def _start_draw(self, context):
        for area in context.screen.areas:
            if area.type == "VIEW_3D":
                self._view_3d_area = area
                break

        for region in self._view_3d_area.regions:
            if region.type == "WINDOW":
                self._window_region = region
                break

        """TODO: 描画処理は後回し"""
        return

        self._user_drawing_callback_func = self._window_region.callback_add(
                _user_drawing_callback, (
                    self._view_3d_area,
                    self._window_region
                ),
                'POST_PIXEL'
        )

        _user_drawing_callback(self._view_3d_area, self._window_region)

    def _stop_draw(self, context):
        self._window_region.callback_remove(self._user_drawing_callback_func)
        self._view_3d_area.tag_redraw()


def _user_drawing_callback(view_3d_area, window_region):
    print("_draw_callback")
    view_3d_area.tag_redraw()


class KinectDataApplyingOperator(bpy.types.Operator):

    bl_idname = "amk2b.kinect_data_applying_operator"
    bl_label = "Kinect Data Applying To Bone Start and Stop"

    def execute(self, context):
        if not bpy.amk2b.kinect_data_applying_started:
            self._start_draw(context)
            bpy.amk2b.kinect_data_applying_started = True
        else:
            self._stop_draw(context)
            bpy.amk2b.kinect_data_applying_started = False
        return {"FINISHED"}
