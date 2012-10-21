from mathutils import Vector
import bpy


class BlenderDataManager(object):

    def __init__(self):
        self._target_object = None

    def set_target_object(self):
        self._target_object = bpy.data.objects["Armature"].pose

    def unset_target_object(self):
        self._target_object = None

    def apply_location(self, user):
        if not self._target_object:
            return
        bones = self._target_object.bones
        if not bones:
            return
        for joint in user.joints.values():
            if joint.name not in bones:
                continue
            bone = bones[joint.name]
            if not bone:
                continue

            location = Vector()
            location.x = joint.location.x
            location.y = joint.location.y
            location.z = joint.location.z

            parent = bone
            while not hasattr(parent, "parent"):
                parent = parent.parent
                if not parent:
                    continue
                if not hasattr(parent, "location"):
                    continue
                location.x -= parent.location.x
                location.y -= parent.location.y
                location.z -= parent.location.z

            bone.location = location
