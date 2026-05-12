import sys, json, time
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills

def ok(label, res):
    success = res.get('success', False)
    s = "✅" if success else "❌"
    print(f"{s} {label}")
    if not success:
        print(f"   ERROR: {res.get('error', str(res))}")
    return success

print("=" * 60)
print("STEP 4a: Gán layer Ground cho Plane")
print("=" * 60)
res = unity_skills.call_skill('gameobject_set_layer_batch', items=json.dumps([
    {"name": "Plane", "layer": "Ground", "recursive": True}
]))
ok("Set Plane layer = Ground", res)
print("  Result:", json.dumps(res))

print("\n" + "=" * 60)
print("STEP 4b: Add SeedPlacer component lên Player")
print("=" * 60)

# Kiểm tra schema component_add
schema = unity_skills.get_skill_schema()
skills = schema.get('skills', [])
for s in skills:
    if s['name'] == 'component_add':
        print("component_add params:", [p['name'] for p in s.get('parameters', [])])
        break

res = unity_skills.call_skill('component_add', 
    name='Player', 
    componentType='SeedPlacer'
)
ok("Add SeedPlacer to Player", res)
print("  Result:", json.dumps(res))
