import sys, json, time
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills

def run():
    # 1. Create a Cylinder
    print("Creating Cylinder...")
    res = unity_skills.call_skill('gameobject_create', name='PlantedTreeTemp', primitiveType='Cylinder')
    if not res.get('success'):
        print("Failed to create cylinder:", res)
        return

    # 2. Add PlantedTree component
    print("Adding PlantedTree component...")
    res = unity_skills.call_skill('component_add', name='PlantedTreeTemp', componentType='PlantedTree')
    if not res.get('success'):
        print("Failed to add PlantedTree component:", res)

    # 3. Create Prefab
    print("Creating Prefab...")
    res = unity_skills.call_skill('prefab_create', name='PlantedTreeTemp', savePath='Assets/_Project/Prefabs/PlantedTree.prefab')
    if not res.get('success'):
        print("Failed to create prefab:", res)
    else:
        print("Successfully created prefab!")

    # 4. Cleanup
    print("Cleaning up...")
    unity_skills.call_skill('gameobject_delete', name='PlantedTreeTemp')

run()
