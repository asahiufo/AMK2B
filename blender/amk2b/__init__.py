from .common import ComObject
from .operators import KinectDataApplyingOperator
from .operators import KinectDataReceivingOperator
from .operators import RecordingOperator
from .panels import AMK2BPanel
import bpy

bl_info = {
    "name": "AMK2B - Kinect Data Receiver",
    "description": "for KinectDataSender.",
    "author": "asahiufo@AM902",
    "version": (1, 0, 0, 0),
    "blender": (2, 64, 0),
    "location": "3D View > Tool Shelf > AMK2B Panel",
    "warning": "",
    "wiki_url": "",
    "tracker_url": "",
    "category": "Development"
}


def register():
    bpy.amk2b = ComObject()
    bpy.amk2b.kinect_data_receiving_started = False
    bpy.amk2b.kinect_data_applying_started = False
    bpy.amk2b.recording_pre_started = False
    bpy.amk2b.recording_started = False

    bpy.utils.register_class(AMK2BPanel)
    bpy.utils.register_class(KinectDataReceivingOperator)
    bpy.utils.register_class(KinectDataApplyingOperator)
    bpy.utils.register_class(RecordingOperator)


def unregister():
    bpy.utils.unregister_class(RecordingOperator)
    bpy.utils.unregister_class(KinectDataApplyingOperator)
    bpy.utils.unregister_class(KinectDataReceivingOperator)
    bpy.utils.unregister_class(AMK2BPanel)
    del bpy.amk2b


if __name__ == "__main__":
    register()
