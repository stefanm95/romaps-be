import geopandas as gpd

# Load your file
gdf = gpd.read_file("DNS-concurencies.geojson")

# Check for exact duplicate geometries
duplicates = gdf[gdf.duplicated("geometry")]

# Remove exact duplicates
gdf_cleaned = gdf.drop_duplicates("geometry")

# Optional: remove duplicate refs too if needed
gdf_cleaned = gdf_cleaned.drop_duplicates(subset=["ref", "geometry"])

# Save cleaned GeoJSON
gdf_cleaned.to_file("cleaned_national_roads.geojson", driver="GeoJSON")
