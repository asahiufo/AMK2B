import bpy


class BlenderDataManager(object):

    def __init__(self):
        self._target_object = None

    def set_target_object(self):
        self._target_object = bpy.context.active_object

    def unset_target_object(self):
        self._target_object = None

    def apply_location(self, user):
        if not self._target_object:
            return
        bones = self._target_object.pose.bones
        if not bones:
            return
        for joint in user.joints.values():
            bone = bones[joint.name]
            if not bone:
                continue
            bone.location = joint.location
