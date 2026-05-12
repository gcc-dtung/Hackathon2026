import sys, json
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills

# Hierarchy đầy đủ dạng tree
print("=== FULL HIERARCHY (depth=1) ===")
ctx = unity_skills.call_skill('hierarchy_describe', maxDepth=1, maxItemsPerLevel=100)
print(json.dumps(ctx, indent=2))
