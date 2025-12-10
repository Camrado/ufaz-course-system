-- Clean existing data before inserting

TRUNCATE TABLE "courses" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "languages" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "categories" RESTART IDENTITY CASCADE;

-- Categories

INSERT INTO "categories" ("id", "name", "created_at", "updated_at", "deleted_at")
VALUES
    (1,  'Mathematics',       '2025-04-20 08:15:00+00', '2025-05-15 14:30:00+00', NULL),
    (2,  'Physics',           '2025-04-22 10:45:00+00', '2025-05-17 09:00:00+00', NULL),
    (3,  'Computer Science',  '2025-04-25 09:00:00+00', '2025-05-20 11:20:00+00', NULL),
    (4,  'Literature',        '2025-04-27 16:30:00+00', '2025-05-23 17:45:00+00', NULL),
    (5,  'History',           '2025-05-01 12:00:00+00', '2025-05-25 08:00:00+00', '2025-06-10 00:00:00+00'),
    (6,  'Biology',           '2025-05-03 14:20:00+00', '2025-05-26 13:15:00+00', NULL),
    (7,  'Chemistry',         '2025-05-06 07:50:00+00', '2025-05-27 09:30:00+00', '2025-06-12 18:00:00+00'),
    (8,  'Philosophy',        '2025-05-10 19:10:00+00', '2025-05-28 16:45:00+00', NULL),
    (9,  'Economics',         '2025-05-12 11:35:00+00', '2025-05-29 10:05:00+00', NULL),
    (10, 'Art',               '2025-05-14 15:25:00+00', '2025-05-30 14:50:00+00', NULL);

-- Reset the sequence after seeding
SELECT setval(pg_get_serial_sequence('categories', 'id'), (SELECT MAX(id) FROM categories));



-- Languages

INSERT INTO "languages" ("id", "code", "created_at", "updated_at", "deleted_at")
VALUES
    (1,  'en', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (2,  'fr', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (3,  'de', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (4,  'es', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', '2025-04-23 08:00:00+00'),
    (5,  'ru', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (6,  'zh', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', '2025-04-24 08:00:00+00'),
    (7,  'az', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (8,  'it', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL),
    (9,  'tr', '2025-04-20 08:00:00+00', '2025-04-20 08:00:00+00', NULL);

-- Reset the sequence after seeding
SELECT setval(pg_get_serial_sequence('languages', 'id'), (SELECT MAX(id) FROM "languages"));



-- Courses

INSERT INTO "courses"
    ("id",
     "name",
     "short_description",
     "description",
     "category_id",
     "language_id",
     "question_answer_count",
     "is_active",
     "slug",
     "created_at",
     "updated_at",
     "deleted_at")
VALUES
    (1,
     'Introduction to Mathematics',
     'Learn the basics of mathematics.',
     'This course covers fundamental concepts in mathematics including algebra, geometry, and calculus.',
     1,
     1,
     0,
     true,
     'introduction-to-mathematics',
     '2025-04-20 08:15:00+00',
     '2025-05-15 14:30:00+00',
     NULL),
    (2,
     'Advanced Physics',
     'Explore advanced topics in physics.',
     'This course delves into quantum mechanics, relativity, and thermodynamics.',
     2,
     2,
     5,
     true,
     'advanced-physics',
     '2025-04-22 10:45:00+00',
     '2025-05-17 09:00:00+00',
     NULL),
    (3,
     'Computer Science Fundamentals',
     'Get started with computer science.',
     'Learn programming basics, data structures, and algorithms in this introductory course.',
     3,
     5,
     4,
     true,
     'computer-science-fundamentals',
     '2025-04-25 09:00:00+00',
     '2025-05-20 11:20:00+00',
     NULL),
    (4,
    'World Literature',
    'Discover classic and modern literature.',
    'This course explores significant works of literature from around the world, analyzing themes and styles.',
    4,
    3,
    2,
    true,
    'world-literature',
    '2025-04-27 16:30:00+00',
    '2025-05-23 17:45:00+00',
    NULL),
    (5,
    'History of Civilizations',
    'A comprehensive overview of world history.',
    'This course covers major civilizations and their impacts on the modern world.',
    5,
    4,
    3,
    true,
    'history-of-civilizations',
    '2025-05-01 12:00:00+00',
    '2025-05-25 08:00:00+00',
    NULL),
    (6,
    'Introduction to Biology',
    'Learn the fundamentals of biology.',
    'This course introduces the basic concepts of biology, including cell structure, genetics, and evolution.',
    6,
    6,
    1,
    true,
    'introduction-to-biology',
    '2025-05-03 14:20:00+00',
    '2025-05-26 13:15:00+00',
    '2025-05-27 13:15:00+00');

-- Reset the sequence after seeding
SELECT setval(pg_get_serial_sequence('courses', 'id'), (SELECT MAX(id) FROM "courses"));
