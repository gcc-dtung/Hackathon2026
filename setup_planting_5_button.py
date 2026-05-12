import sys, json, time
sys.path.append(r'C:\Users\Admin\.gemini\antigravity\skills\unity-skills\scripts')
import unity_skills

def run():
    print("Waiting for compilation...")
    time.sleep(5) # short wait for Unity to detect the file change
    
    # Wait until compilation is done
    for _ in range(30):
        diag = unity_skills.call_skill('debug_check_compilation')
        if not diag.get('isCompiling', True):
            break
        time.sleep(1)
        
    print("Running Setup Planting Button...")
    res = unity_skills.call_skill('editor_execute_menu', menuPath='Tools/Setup Planting Button (One-Shot)')
    print(json.dumps(res, indent=2))

run()
