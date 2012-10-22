import bpy


class AMK2BPanel(bpy.types.Panel):

    bl_label = "AMK2B"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'TOOLS'

    @classmethod
    def poll(cls, context):
        return hasattr(bpy, "amk2b")

    def draw(self, context):
        layout = self.layout

        row = layout.row()
        row.label(text="Receive Kinect Data")
        row = layout.row()
        if not bpy.amk2b.kinect_data_receiving_started:
            row.operator("amk2b.kinect_data_receiving_operator", text="Start")
        else:
            row.operator("amk2b.kinect_data_receiving_operator", text="Stop")

        row = layout.row()
        row.label(text="Recording")
        row = layout.row()
        if not bpy.amk2b.recording_started:
            row.operator("amk2b.recording_operator", text="Start")
        else:
            row.operator("amk2b.recording_operator", text="Stop")
