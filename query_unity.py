import sys
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills
import json

res_objs = unity_skills.call_skill('scene_find_objects', namePattern='Ground')
print('Grounds:', json.dumps(res_objs))

res_terrain = unity_skills.call_skill('scene_find_objects', namePattern='Terrain')
print('Terrains:', json.dumps(res_terrain))

res_player = unity_skills.call_skill('scene_find_objects', namePattern='Player')
print('Players:', json.dumps(res_player))

res_btn = unity_skills.call_skill('scene_find_objects', namePattern='Plant')
print('Buttons Plant:', json.dumps(res_btn))
