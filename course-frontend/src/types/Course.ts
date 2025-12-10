// ===== GET /api/courses (Get All)
export interface GetAllCoursesResponse {
  courses: GetCourseResponse[];
}

// ===== GET /api/courses/{id}
export interface GetCourseResponse {
  id: number;
  name: string;
  shortDescription: string;
  description: string;
  categoryId?: number;
  categoryName?: string;
  languageId?: number;
  languageCode?: string;
  questionAnswerCount?: number;
  isActive: boolean;
  slug?: string;
  createdAt: string;
  updatedAt: string;
}

// ===== Represents a course inside lists (you may reuse GetCourseResponse too)
export interface Course {
  id: number;
  name: string;
  shortDescription?: string;
  description?: string;
  
  categoryId?: number | null;    // <--- FIX
  categoryName?: string | null;

  languageId?: number | null;     // <--- FIX
  languageCode?: string | null;   // <--- returned only

  questionAnswerCount?: number;
  isActive: boolean;
  slug?: string;
  createdAt: string;
  updatedAt: string;
}

// ===== POST /api/courses (Create)
export interface CreateCourseRequest {
  name: string;
  shortDescription?: string;
  description?: string;
  categoryId: number;
  languageId: number;
  questionAnswerCount?: number;
  isActive: boolean;
  slug?: string;
}

export interface CreateCourseResponse {
  id: number;
}

// ===== PUT /api/courses/{id} (Update)
export interface UpdateCourseRequest {
  courseId: number;
  name: string;
  shortDescription?: string;
  description?: string;
  categoryId: number;
  languageId: number;
  questionAnswerCount?: number;
  isActive: boolean;
  slug?: string;
}

export interface UpdateCourseResponse {
  id: number;
}

// ===== DELETE /api/courses/{id}
export interface DeleteCourseResponse {
  id: number;
}
