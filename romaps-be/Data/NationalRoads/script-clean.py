import json

# List of keys to keep in properties
keys_to_keep = [
    'id',
    'ref',
    'nat_ref',
    'int_ref',
    'highway',
    'lanes',
    'maxspeed',
    'maxspeed:hgv',
    'surface',
    'smoothness',
    'oneway',
    'source',
    'source:surface',
    'source:maxspeed',
    'source:date'
]

def clean_properties(properties):
    return {k: v for k, v in properties.items() if k in keys_to_keep and v not in (None, '', [], {})}

def clean_geojson(input_file, output_file):
    with open(input_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    if 'features' not in data or not isinstance(data['features'], list):
        print("Invalid GeoJSON: missing features array")
        return

    for feature in data['features']:
        if 'properties' in feature:
            feature['properties'] = clean_properties(feature['properties'])

    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)

    print(f"Cleaned GeoJSON saved to {output_file}")

# Usage example:
input_path = 'cleaned_national_roads.geojson'
output_path = 'cleaned-DNS-concurencies.geojson'

clean_geojson(input_path, output_path)
