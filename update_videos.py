import requests
import json
from pathlib import Path

# CONFIGURATION
API_KEY = "AIzaSyCyh_Y1rpiLpKEg-a1kFhDQLkp1fiVMrRU"  # ta clé API
PLAYLIST_ID = "PLpkdMUKG2c0Lg-YtG1Qms4WBFD96THbZV"
MAX_RESULTS = 50  # limite max de l'API par appel

# URL de l'API pour récupérer les vidéos d'une playlist
YOUTUBE_API_URL = (
    f"https://www.googleapis.com/youtube/v3/playlistItems"
    f"?part=snippet&maxResults={MAX_RESULTS}&playlistId={PLAYLIST_ID}&key={API_KEY}"
)

# Récupération des vidéos
response = requests.get(YOUTUBE_API_URL)
data = response.json()

videos = []

for item in data.get("items", []):
    snippet = item["snippet"]
    title = snippet["title"]
    video_id = snippet["resourceId"]["videoId"]
    embed_url = f"https://www.youtube.com/embed/{video_id}"
    videos.append({
        "title": title,
        "url": embed_url
    })

# Sauvegarde dans le fichier JSON
json_path = Path("videos.json")
with open(json_path, "w", encoding="utf-8") as f:
    json.dump({"videos": videos}, f, ensure_ascii=False, indent=4)

print(f"{len(videos)} vidéos enregistrées depuis la playlist dans {json_path}")
