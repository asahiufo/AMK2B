from mathutils import Vector
import bpy


class BlenderDataManager(object):

    def apply_location(self, user):
        for joint in user.joints.values():
            for obj in bpy.data.objects:
                if obj.type != "ARMATURE":
                    continue
                armature = obj
                if not armature.pose:
                    continue
                pose = armature.pose
                if not pose.bones:
                    continue
                bones = pose.bones
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

                if bpy.amk2b.recording_started:
                    bone.keyframe_insert(
                        data_path="location",
                        frame=bpy.context.scene.frame_current
                    )
