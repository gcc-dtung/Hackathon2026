import json

with open('skills.json', 'r') as f:
    data = json.load(f)

for skill in data.get('skills', []):
    name = skill.get('name', '')
    if 'layer' in name.lower() or 'tag' in name.lower() or 'project' in name.lower():
        print(name)
