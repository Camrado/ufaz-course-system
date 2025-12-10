import axios from "axios";
import type {
  GetLanguageResponse,
  GetAllLanguagesResponse,
  CreateLanguageRequest,
  CreateLanguageResponse,
  UpdateLanguageRequest,
  UpdateLanguageResponse,
  DeleteLanguageResponse
} from "../types/Language";

const API_URL = import.meta.env.VITE_API_URL;


// ========= GET ALL =========
export const getAllLanguages = async (): Promise<GetAllLanguagesResponse> => {
  const res = await axios.get<GetAllLanguagesResponse>(`${API_URL}/languages`);
  return res.data;
};


// ========= GET ONE =========
export const getLanguageById = async (languageId: number): Promise<GetLanguageResponse> => {
  const res = await axios.get<GetLanguageResponse>(`${API_URL}/languages/${languageId}`);
  return res.data;
};


// ========= CREATE =========
export const createLanguage = async (
  data: CreateLanguageRequest
): Promise<CreateLanguageResponse> => {
  const res = await axios.post<CreateLanguageResponse>(`${API_URL}/languages`, data);
  return res.data;
};


// ========= UPDATE =========
export const updateLanguage = async (
  data: UpdateLanguageRequest
): Promise<UpdateLanguageResponse> => {
  const res = await axios.put<UpdateLanguageResponse>(
    `${API_URL}/languages/${data.languageId}`,
    data
  );
  return res.data;
};


// ========= DELETE =========
export const deleteLanguage = async (
  languageId: number
): Promise<DeleteLanguageResponse> => {
  const res = await axios.delete<DeleteLanguageResponse>(`${API_URL}/languages/${languageId}`);
  return res.data;
};
