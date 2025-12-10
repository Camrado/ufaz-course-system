// ========= LANGUAGE TYPES =========

// Returned when fetching a single language (GET /api/languages/{id})
export interface GetLanguageResponse {
  id: number;
  code: string;
  createdAt: string;   // DateTime → string in JSON
  updatedAt: string;   // DateTime → string in JSON
}

// Returned when fetching ALL languages (GET /api/languages)
export interface GetAllLanguagesResponse {
  languages: GetLanguageResponse[];
}

export interface Language {
  id: number;
  code: string;
  createdAt: string;
  updatedAt: string;
}


// ----- CREATE LANGUAGE -----
export interface CreateLanguageRequest {
  code: string;  // matches CreateLanguageCommand(string Code)
}

export interface CreateLanguageResponse {
  id: number;    // CreateLanguageCommandResponse(int Id)
}


// ----- UPDATE LANGUAGE -----
export interface UpdateLanguageRequest {
  languageId: number; // must match backend exactly
  code: string;
}

export interface UpdateLanguageResponse {
  id: number;
}


// ----- DELETE LANGUAGE -----
export interface DeleteLanguageResponse {
  id: number;
}
