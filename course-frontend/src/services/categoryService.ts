import axios from "axios";
import type {
  GetCategoryResponse,
  GetAllCategoriesResponse,
  CreateCategoryRequest,
  CreateCategoryResponse,
  UpdateCategoryRequest,
  UpdateCategoryResponse,
  DeleteCategoryResponse
} from "../types/Category";

const API_URL = import.meta.env.VITE_API_URL;


// ========= GET ALL =========
export const getAllCategories = async (): Promise<GetAllCategoriesResponse> => {
  const res = await axios.get<GetAllCategoriesResponse>(`${API_URL}/categories`);
  return res.data;
};


// ========= GET ONE =========
export const getCategoryById = async (categoryId: number): Promise<GetCategoryResponse> => {
  const res = await axios.get<GetCategoryResponse>(`${API_URL}/categories/${categoryId}`);
  return res.data;
};


// ========= CREATE =========
export const createCategory = async (
  data: CreateCategoryRequest
): Promise<CreateCategoryResponse> => {
  const res = await axios.post<CreateCategoryResponse>(`${API_URL}/categories`, data);
  return res.data;
};


// ========= UPDATE =========
export const updateCategory = async (
  data: UpdateCategoryRequest
): Promise<UpdateCategoryResponse> => {
  const res = await axios.put<UpdateCategoryResponse>(
    `${API_URL}/categories/${data.categoryId}`,
    data
  );
  return res.data;
};


// ========= DELETE =========
export const deleteCategory = async (
  categoryId: number
): Promise<DeleteCategoryResponse> => {
  const res = await axios.delete<DeleteCategoryResponse>(`${API_URL}/categories/${categoryId}`);
  return res.data;
};
