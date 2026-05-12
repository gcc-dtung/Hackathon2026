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

# Đúng params: propertyName, value/referencePath/referenceName/assetPath
print("=== Step 4c: Configure SeedPlacer fields ===")

# playerCamera = reference to Main Camera (child of Player)
res = unity_skills.call_skill('component_set_property',
    name='Player',
    componentType='SeedPlacer',
    propertyName='playerCamera',
    referencePath='Player/Main Camera'
)
ok("SeedPlacer.playerCamera = Main Camera", res)
print("  Detail:", json.dumps(res))

# placeRange = 10
res = unity_skills.call_skill('component_set_property',
    name='Player',
    componentType='SeedPlacer',
    propertyName='placeRange',
    value='10'
)
ok("SeedPlacer.placeRange = 10", res)

# groundLayer = 256 (Ground layer, 1<<8)
res = unity_skills.call_skill('component_set_property',
    name='Player',
    componentType='SeedPlacer',
    propertyName='groundLayer',
    value='256'
)
ok("SeedPlacer.groundLayer = 256 (Ground)", res)
print("  Detail:", json.dumps(res))
