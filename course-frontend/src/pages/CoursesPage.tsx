import { useEffect, useState } from "react";
import { BookOpen, Plus, Edit2, Trash2, ArrowLeft } from "lucide-react";
import { getAllCourses, createCourse, updateCourse, deleteCourse } from "../services/courseService";
import { getAllLanguages } from "../services/languageService";
import type { Course } from "../types/Course";
import type { Category } from "../types/Category";
import type { Language } from "../types/Language";

export default function CoursesPage({ category, onBack }: { category: Category, onBack: () => void }) {

  const [courses, setCourses] = useState<Course[]>([]);
  const [languages, setLanguages] = useState<Language[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [edit, setEdit] = useState<Course | null>(null);

  const [form, setForm] = useState({
    name: "",
    shortDescription: "",
    description: "",
    languageId: "",
    questionAnswerCount: "",
    slug: "",
    isActive: false,
  });

  const [toast, setToast] = useState<{ msg: string, type: "success" | "error" } | null>(null);

  const show = (msg: string, type: "success" | "error") => {
    setToast({ msg, type });
    setTimeout(()=>setToast(null),2500);
  };

  const load = async () => {
    try {
      const courseRes = await getAllCourses();
      const langRes = await getAllLanguages();

      setLanguages(langRes.languages);
      setCourses(courseRes.courses.filter(c => c.categoryId === category.id));
    } catch {
      show("Failed to load data", "error");
    }
  };
  useEffect(()=>{ load(); },[]);


  // reset when modal opens
  const openCreate = () => {
    setEdit(null);
    setForm({
      name:"",
      shortDescription:"",
      description:"",
      languageId:"",
      questionAnswerCount:"",
      slug:"",
      isActive:false,
    });
    setOpenModal(true);
  };


  const submit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
        name: form.name,
        shortDescription: form.shortDescription || undefined,
        description: form.description || undefined,
        categoryId: category.id,
        languageId: Number(form.languageId),     // ⬅ ALWAYS A NUMBER
        questionAnswerCount: form.questionAnswerCount ? Number(form.questionAnswerCount) : undefined,
        slug: form.slug || undefined,
        isActive: form.isActive,
    };


    try {
      if(edit){
        await updateCourse({ courseId: edit.id, ...payload });
        show("Course updated","success");
      }else{
        await createCourse(payload);
        show("Course created","success");
      }
      setOpenModal(false);
      load();
    } catch {
      show("Error saving","error");
    }
  };


  const remove = async (id: number) => {
    try {
      await deleteCourse(id);      // <— delete immediately, no popup
      await load();                // refresh list
    } catch (err) {
      console.error(err);
      alert("Delete failed — the course may already be removed.");
    }
  };


  const startEdit = (c: Course) => {
    setEdit(c);
    setForm({
      name: c.name,
      shortDescription: c.shortDescription ?? "",
      description: c.description ?? "",
      languageId: c.languageId?.toString() || "",
      questionAnswerCount: c.questionAnswerCount?.toString() || "",
      slug: c.slug ?? "",
      isActive: c.isActive,
    });
    setOpenModal(true);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-50 to-pink-100 p-8">

      {/* Back Button */}
      <button onClick={onBack} className="flex gap-2 text-gray-700 hover:text-black mb-6">
        <ArrowLeft size={18}/> Back to Categories
      </button>


      {/* Header */}
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="flex items-center gap-3 text-4xl font-bold text-gray-800">
            <BookOpen size={42} className="text-purple-600"/>
            {category.name} Courses
          </h1>
          <p className="text-gray-600">{courses.length} courses in this category</p>
        </div>

        <button
          onClick={openCreate}
          className="flex items-center gap-2 bg-purple-600 text-white px-6 py-3 rounded-lg shadow hover:bg-purple-700"
        >
          <Plus size={20}/> Add Course
        </button>
      </div>


      {/* Course Cards */}
      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">

        {courses.map(c => (
          <div key={c.id} className="bg-white rounded-xl shadow-md hover:shadow-2xl p-6 border border-purple-200">

            <div className="flex justify-between mb-2">
              <h2 className="text-lg font-bold text-gray-900">{c.name}</h2>
              {c.isActive && (
                <span className="text-xs px-2 py-1 bg-green-100 text-green-700 rounded-md font-medium">
                  Active
                </span>
              )}
            </div>

            {c.shortDescription && <p className="text-sm text-gray-600">{c.shortDescription}</p>}

            <div className="mt-3 text-sm text-gray-500 space-y-1">
              {c.languageCode && <p>🌍 Language: <b>{c.languageCode.toUpperCase()}</b></p>}
              {c.questionAnswerCount !== undefined && <p>📝 {c.questionAnswerCount} Q&A</p>}
              {c.slug && <p className="text-purple-600 font-mono text-xs">/{c.slug}</p>}
            </div>

            {/* Buttons */}
            <div className="flex gap-2 pt-5">
              <button className="flex-1 bg-gray-100 hover:bg-gray-200 p-2 rounded"
                onClick={()=>startEdit(c)}
              >
                <Edit2 size={16}/> Edit
              </button>

              <button className="flex-1 bg-red-100 text-red-700 hover:bg-red-200 p-2 rounded"
                onClick={()=>remove(c.id)}
              >
                <Trash2 size={16}/> Delete
              </button>
            </div>

          </div>
        ))}

        {courses.length === 0 && (
          <p className="text-center text-gray-500 col-span-full">No courses yet...</p>
        )}
      </div>


      {/* ------ MODAL FORM ------ */}
      {openModal && (
        <div className="fixed inset-0 bg-black/40 flex justify-center items-center p-4">
          <div className="bg-white p-6 rounded-xl w-full max-w-lg shadow">

            <h2 className="text-xl font-bold mb-4">{edit ? "Edit Course" : "Create Course"}</h2>

            <form onSubmit={submit} className="space-y-3">

              <input required placeholder="Course name"
                className="border p-2 w-full rounded"
                value={form.name}
                onChange={e=>setForm({...form, name:e.target.value})}
              />

              <input placeholder="Short description"
                className="border p-2 w-full rounded"
                value={form.shortDescription}
                onChange={e=>setForm({...form, shortDescription:e.target.value})}
              />

              <textarea placeholder="Description"
                className="border p-2 w-full rounded"
                rows={3}
                value={form.description}
                onChange={e=>setForm({...form, description:e.target.value})}
              />

              <select required
                className="border p-2 w-full rounded"
                value={form.languageId}
                onChange={e=>setForm({...form, languageId:e.target.value})}
              >
                <option value="">Select language</option>
                {languages.map(l=>(
                  <option key={l.id} value={l.id}>{l.code.toUpperCase()}</option>
                ))}
              </select>

              <input type="number" min="0" placeholder="Q&A Count"
                className="border p-2 w-full rounded"
                value={form.questionAnswerCount}
                onChange={e=>setForm({...form,questionAnswerCount:e.target.value})}
              />

              <input placeholder="Slug (optional)"
                className="border p-2 w-full rounded font-mono"
                value={form.slug}
                onChange={e=>setForm({...form, slug:e.target.value})}
              />

              <label className="flex gap-2 items-center">
                <input type="checkbox"
                  checked={form.isActive}
                  onChange={e=>setForm({...form, isActive:e.target.checked})}
                />
                Active
              </label>

              <button className="bg-purple-600 text-white w-full py-2 rounded-lg hover:bg-purple-700 mt-4">
                {edit ? "Update" : "Create"}
              </button>

              <button
                type="button"
                onClick={() => setOpenModal(false)}
                className="w-full py-2 rounded text-gray-600 hover:text-gray-900 text-sm">
                Cancel
              </button>

            </form>
          </div>
        </div>
      )}

      {/* Toast */}
      {toast && (
        <div className={`fixed top-5 right-5 px-4 py-2 rounded text-white ${toast.type==="success"?"bg-green-500":"bg-red-500"}`}>
          {toast.msg}
        </div>
      )}

    </div>
  );
}
