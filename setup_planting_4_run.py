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

# Kiểm tra compile trước khi chạy
print("=== Kiểm tra compile lỗi ===")
feedback = unity_skills.call_skill('debug_check_compilation')
print(json.dumps(feedback, indent=2))

print("\n=== Chạy Tools > Setup SeedPlacer ===")
res = unity_skills.call_skill('editor_execute_menu', menuPath='Tools/Setup SeedPlacer (One-Shot)')
ok("Execute menu: Setup SeedPlacer", res)
print("  Result:", json.dumps(res))
