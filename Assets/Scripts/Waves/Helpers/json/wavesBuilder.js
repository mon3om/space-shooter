const fs = require("fs");
const SHAPES = {
  InRandomYPosition: "InRandomYPosition",
  InVShape: "InVShape",
  InWaveShape: "InWaveShape",
  InIShape: "InIShape",
};

const ENTER_POSITIONS = {
  ShouldEnterFromTop: "ShouldEnterFromTop",
  ShouldEnterFromBottom: "ShouldEnterFromBottom",
  ShouldEnterFromLeft: "ShouldEnterFromLeft",
  ShouldEnterFromRight: "ShouldEnterFromRight",
  ShouldEnterFromVerticalCornersMiddle: "ShouldEnterFromVerticalCornersMiddle",
  ShouldEnterFromVerticalCornersLeft: "ShouldEnterFromVerticalCornersLeft",
  ShouldEnterFromVerticalCornersRight: "ShouldEnterFromVerticalCornersRight",
  ShouldEnterFromHorizontalCornersTop: "ShouldEnterFromHorizontalCornersTop",
  ShouldEnterFromHorizontalCornersCenter:
    "ShouldEnterFromHorizontalCornersCenter",
  ShouldEnterFromHorizontalCornersBottom:
    "ShouldEnterFromHorizontalCornersBottom",
  ShouldEnterFromRandom: "ShouldEnterFromRandom",
};

const EASY = [
  {
    enemyPrefab: "Enemies/LaserWave-EASY-2",
    count: { min: 1, max: 1, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/LaserEnemy",
    count: { min: 1, max: 1, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/Asteroid",
    count: { min: 1, max: 5, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromRandom],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/PeacefulEnemy",
    count: { min: 4, max: 16, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [
      SHAPES.InIShape,
      SHAPES.InVShape,
      SHAPES.InWaveShape,
      SHAPES.InRandomYPosition,
      SHAPES.InRandomYPosition,
    ],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/PeacefulEnemy",
    count: { min: 2, max: 6, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromLeft,
      ENTER_POSITIONS.ShouldEnterFromRight,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/PeacefulShootingEnemy",
    count: { min: 2, max: 8, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [
      SHAPES.InIShape,
      SHAPES.InVShape,
      SHAPES.InWaveShape,
      SHAPES.InRandomYPosition,
      SHAPES.InRandomYPosition,
    ],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/PeacefulShootingEnemy",
    count: { min: 2, max: 5, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromLeft,
      ENTER_POSITIONS.ShouldEnterFromRight,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
];

const MODERATE = [
  {
    enemyPrefab: "Enemies/LaserEnemy",
    count: { min: 2, max: 2, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/Asteroid-MEDIUM",
    count: { min: 5, max: 10, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromRandom],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/LaserWave-EASY-1",
    count: { min: 1, max: 1, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/PeacefulTrackerEnemy",
    count: { min: 4, max: 10, step: 2 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromVerticalCornersMiddle,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/ShootingEnemy",
    count: { min: 2, max: 4, step: 1 },
    enterPositions: [ENTER_POSITIONS.ShouldEnterFromTop],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/EnhancedShootingEnemy",
    count: { min: 1, max: 2, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/EnhancedShootingEnemy",
    count: { min: 1, max: 1, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/KamikazeTrackerEnemy",
    count: { min: 2, max: 4, step: 2 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromVerticalCornersMiddle,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersCenter,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersBottom,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromBottom,
      ENTER_POSITIONS.ShouldEnterFromLeft,
      ENTER_POSITIONS.ShouldEnterFromRight,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
];

const HARD = [
  {
    enemyPrefab: "Enemies/KamikazeTrackerEnemy",
    count: { min: 8, max: 12, step: 2 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromVerticalCornersMiddle,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersCenter,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersBottom,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromBottom,
      ENTER_POSITIONS.ShouldEnterFromLeft,
      ENTER_POSITIONS.ShouldEnterFromRight,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/KamikazeTrackerEnemy",
    count: { min: 2, max: 6, step: 2 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromVerticalCornersMiddle,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersCenter,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersBottom,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromBottom,
      ENTER_POSITIONS.ShouldEnterFromLeft,
      ENTER_POSITIONS.ShouldEnterFromRight,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/ShootingEnemy",
    count: { min: 2, max: 8, step: 2 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromTop,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersCenter,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/EnhancedShootingEnemy",
    count: { min: 2, max: 5, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
  {
    enemyPrefab: "Enemies/StarsEnemy",
    count: { min: 1, max: 1, step: 1 },
    enterPositions: [
      ENTER_POSITIONS.ShouldEnterFromHorizontalCornersTop,
      ENTER_POSITIONS.ShouldEnterFromTop,
    ],
    shapes: [],
    maxEnemiesPerLine: -1,
  },
];

const jsonString = JSON.stringify({
  data: [
    ...EASY.map((el) => {
      el.difficulty = "Easy";
      el.countMin = el.count.min;
      el.countMax = el.count.max;
      el.countStep = el.count.step;
      delete el.count;
      return el;
    }),
    ...MODERATE.map((el) => {
      el.difficulty = "Moderate";
      el.countMin = el.count.min;
      el.countMax = el.count.max;
      el.countStep = el.count.step;
      delete el.count;
      return el;
    }),
    ...HARD.map((el) => {
      el.difficulty = "Hard";
      el.countMin = el.count.min;
      el.countMax = el.count.max;
      el.countStep = el.count.step;
      delete el.count;
      return el;
    }),
  ],
});

const filePath = "D:/Projects/Unity/Space Shooter/Assets/Resources/waves.json";

fs.writeFile(filePath, jsonString, (err) => {
  if (err) {
    console.error("Error writing file:", err);
  } else {
    console.log("JSON file has been saved.");
  }
});
