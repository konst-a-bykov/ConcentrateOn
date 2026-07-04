#!/bin/bash
# Encode AncientMan animation assets for web
# Source: ConcentrateOn/Animations/AncientMan/*.mp4
# Target: concentrateon-web/public/animations/ancient-man/*.webm

set -euo pipefail

PROJECT_ROOT="/Users/konstantinbykov/Documents/Data/MyProjects/ConcentrateOn"
SRC="$PROJECT_ROOT/ConcentrateOn/Animations/AncientMan"
SRC_AUDIO="$PROJECT_ROOT/ConcentrateOn/Animations"
DEST="$PROJECT_ROOT/concentrateon-web/public/animations/ancient-man"

mkdir -p "$DEST"

echo "=== Encoding MP4 -> WebM (VP9) ==="
echo ""

encode() {
  local src_file="$1"
  local dest_file="$2"
  if [ ! -f "$src_file" ]; then
    echo "SKIP: $src_file not found"
    return
  fi
  echo "Encoding: $(basename "$src_file") -> $(basename "$dest_file")"
  ffmpeg -y -i "$src_file" \
    -c:v libvpx-vp9 \
    -crf 35 \
    -b:v 0 \
    -an \
    -row-mt 1 \
    "$dest_file" 2>/dev/null
  local src_size=$(du -h "$src_file" | cut -f1)
  local dest_size=$(du -h "$dest_file" | cut -f1)
  echo "  $src_size -> $dest_size"
}

encode "$SRC/Resting.mp4"    "$DEST/resting.webm"
encode "$SRC/RestingV.mp4"   "$DEST/resting-portrait.webm"
encode "$SRC/GoToWork.mp4"   "$DEST/start-working.webm"
encode "$SRC/GoToWorkV.mp4"  "$DEST/start-working-portrait.webm"
encode "$SRC/Working.mp4"    "$DEST/working.webm"
encode "$SRC/WorkingV.mp4"   "$DEST/working-portrait.webm"
encode "$SRC/GoToRest.mp4"   "$DEST/stop-working.webm"
encode "$SRC/GoToRestV.mp4"  "$DEST/stop-working-portrait.webm"

echo ""
echo "=== Copying PNG thumbnails ==="
cp "$SRC/RestImage.png" "$DEST/thumbnail-rest.png"
cp "$SRC/WorkImage.png" "$DEST/thumbnail-work.png"
echo "  thumbnail-rest.png, thumbnail-work.png"

echo ""
echo "=== Encoding WAV -> MP3 ==="
ffmpeg -y -i "$SRC_AUDIO/bell.wav" \
  -codec:a libmp3lame -qscale:a 2 \
  "$DEST/bell.mp3" 2>/dev/null
echo "  bell.wav -> bell.mp3"

ffmpeg -y -i "$SRC_AUDIO/bellFinish.wav" \
  -codec:a libmp3lame -qscale:a 2 \
  "$DEST/bell-finish.mp3" 2>/dev/null
echo "  bellFinish.wav -> bell-finish.mp3"

echo ""
echo "=== Results ==="
du -sh "$DEST"
echo ""
ls -lh "$DEST"
