// ========= CATEGORY TYPES =========

// Returned when fetching a single category (GET /api/categories/{id})
export interface GetCategoryResponse {
  id: number;
  name: string;
  createdAt: string;   // DateTime → string
  updatedAt: string;   // DateTime → string
}

// Returned for fetching all categories (GET /api/categories)
export interface GetAllCategoriesResponse {
  categories: GetCategoryResponse[];
}

export interface Category {
  id: number;
  name: string;
  createdAt: string;
  updatedAt: string;
}

// ----- CREATE CATEGORY -----
export interface CreateCategoryRequest {
  name: string; // matches CreateCategoryCommand(string Name)
}

export interface CreateCategoryResponse {
  id: number;   // matches CreateCategoryCommandResponse(int Id)
}


// ----- UPDATE CATEGORY -----
export interface UpdateCategoryRequest {
  categoryId: number; // important! must match backend exactly
  name: string;
}

export interface UpdateCategoryResponse {
  id: number;
}


// ----- DELETE CATEGORY -----
export interface DeleteCategoryResponse {
  id: number;
}
