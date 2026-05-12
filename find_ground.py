import sys
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills
res = unity_skills.call_skill('scene_find_objects', limit=1000)
for obj in res.get('objects', []):
    name = obj['name'].lower()
    if 'floor' in name or 'ground' in name or 'plane' in name or 'terrain' in name or 'map' in name:
        print(obj['name'], obj['path'])
