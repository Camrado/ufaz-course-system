import axios from "axios";
import type {
  UpdateCourseResponse,
  UpdateCourseRequest,
  CreateCourseResponse,
  CreateCourseRequest,
  DeleteCourseResponse,
  GetCourseResponse,
  GetAllCoursesResponse,
  Course
} from "../types/Course";


const API_URL = import.meta.env.VITE_API_URL;


// =================== GET ALL ===================
export const getAllCourses = async (): Promise<GetAllCoursesResponse> => {
  const res = await axios.get<GetAllCoursesResponse>(`${API_URL}/courses`);
  return res.data;
};


// =================== GET ONE ===================
export const getCourseById = async (courseId: number): Promise<GetCourseResponse> => {
  const res = await axios.get<GetCourseResponse>(`${API_URL}/courses/${courseId}`);
  return res.data;
};


// =================== CREATE ===================
export const createCourse = async (data: CreateCourseRequest): Promise<CreateCourseResponse> => {
  const res = await axios.post<CreateCourseResponse>(`${API_URL}/courses`, data);
  return res.data;
};


// =================== UPDATE ===================
export const updateCourse = async (data: UpdateCourseRequest): Promise<UpdateCourseResponse> => {
  const res = await axios.put<UpdateCourseResponse>(
    `${API_URL}/courses/${data.courseId}`, // URL param uses courseId
    data
  );
  return res.data;
};


// =================== DELETE ===================
export const deleteCourse = async (courseId: number): Promise<DeleteCourseResponse> => {
  const res = await axios.delete<DeleteCourseResponse>(`${API_URL}/courses/${courseId}`);
  return res.data;
};
