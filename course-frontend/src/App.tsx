import { useState } from "react";
import { BookOpen, Globe, Tag } from "lucide-react";
import LanguagesPage from "./pages/LanguagesPage"; 
import CategoriesPage from "./pages/CategoriesPage"; 
import CoursesPage from "./pages/CoursesPage"; 
import type { Category } from "./types/Category";

export default function App() {
  const [currentPage, setCurrentPage] = useState<'languages' | 'categories' | 'courses'>('languages');
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);

  return (
    <>
      <nav className="bg-white shadow sticky top-0 z-30 px-6 h-20 flex items-center justify-between">

    {/* Branding */}
    <div className="flex items-center gap-4">
        <BookOpen size={34} className="text-purple-600" />
        <h1 className="text-[24px] font-semibold tracking-wide">Course System</h1>
    </div>

    {/* Page Switch Buttons */}
    <div className="flex items-center gap-4">

        <button
            onClick={() => setCurrentPage("languages")}
            className={`px-6 py-2.5 rounded-xl flex items-center gap-2 text-[15px] font-medium transition ${
                currentPage === "languages"
                    ? "bg-indigo-600 text-white shadow"
                    : "border text-gray-700 hover:bg-gray-100"
            }`}
        >
            <Globe size={19}/>
            Languages
        </button>

        <button
            onClick={() => setCurrentPage("categories")}
            className={`px-6 py-2.5 rounded-xl flex items-center gap-2 text-[15px] font-medium transition ${
                currentPage === "categories"
                    ? "bg-emerald-600 text-white shadow"
                    : "border text-gray-700 hover:bg-gray-100"
            }`}
        >
            <Tag size={19}/>
            Categories
        </button>

    </div>
</nav>

      {currentPage === "languages" && <LanguagesPage />}
      {currentPage === "categories" && (
        <CategoriesPage onCategorySelect={(c)=>{ setSelectedCategory(c); setCurrentPage("courses"); }}/>
      )}
      {currentPage === "courses" && selectedCategory && (
        <CoursesPage category={selectedCategory} onBack={()=>{ setSelectedCategory(null); setCurrentPage("categories") }} />
      )}
    </>
  );
}
