using UnityEngine;

namespace Level {

	public class StoryLevelLoader : Game.LevelCreator {

		string[] _lines;

		public StoryLevelLoader(int level) {

			loadFile(level.ToString());
		}

		private void loadFile(string filename) {

			TextAsset file = Resources.Load("Levels/" + filename) as TextAsset;

            char[] charSeparators = new char[] {'\n', '\r'};
            _lines = file.text.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);

            if (_lines.Length <= 0) {

                Debug.LogWarning("Error loading file");
                return;
            }

            initProperties();
		}

        private void initProperties() {

            _width = _lines[0].Length;
            _height = _lines.Length - 1;

            _levelData = new int[_width, _height];
            _collisionMap = new Game.TileType[_width, _height];

            for (int x = 0; x < _width; x++) {

                for (int y = 0; y < _height; y++) {

                    _levelData[x, y] = int.Parse(_lines[y][x].ToString());
                }
            }
        }

        public override Vector2 getStartingPoint() {

            string[] points = _lines[_lines.Length - 1].Split(',');

            return new Vector2(int.Parse(points[0]), int.Parse(points[1]));
        }
	}
}