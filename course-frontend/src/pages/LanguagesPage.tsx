import { useEffect, useState } from "react";
import { Globe, Plus, Edit2, Trash2 } from "lucide-react";
import { getAllLanguages, createLanguage, updateLanguage, deleteLanguage } from "../services/languageService";
import type { Language } from "../types/Language";

export default function LanguagesPage() {

  const [languages, setLanguages] = useState<Language[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [edit, setEdit] = useState<Language | null>(null);
  const [code, setCode] = useState("");

  // Load ONCE — no sorting, keeps natural DB order
  const load = async () => {
    try {
      const res = await getAllLanguages();
      setLanguages(res.languages);  // ← keep raw sequence
    } catch {
      console.log("Failed to load languages");
    }
  };

  useEffect(() => { load(); }, []);

  const openCreate = () => {
    setEdit(null);
    setCode("");
    setOpenModal(true);
  };

  const submit = async (e: React.FormEvent) => {
  e.preventDefault();
  try {
    if (edit) {
      await updateLanguage({ languageId: edit.id, code: code.toLowerCase() });
    } else {
      await createLanguage({ code: code.toLowerCase() });
    }
    setOpenModal(false);
    load();
  } catch (err: any) {
    console.log(err.response?.data); // 🔥 show full backend message in console
    alert(
      "Error: " + 
      (err.response?.data?.errors 
        ? JSON.stringify(err.response.data.errors)
        : err.response?.data?.detail ?? err.message)
    );
  }
};


  const startEdit = (l: Language) => {
    setEdit(l);
    setCode(l.code);
    setOpenModal(true);
  };

  const remove = async (id: number) => {
    deleteLanguage(id);
    setLanguages(prev => prev.filter(l => l.id !== id)); // remove without reload
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-indigo-50 to-blue-100 p-10">

      {/* Header */}
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="flex items-center gap-3 text-4xl font-bold text-gray-800">
            <Globe size={40} className="text-indigo-600"/>
            Languages
          </h1>
          <p className="text-gray-600 text-sm">Manage course languages</p>
        </div>

        <button
          onClick={openCreate}
          className="flex items-center gap-2 bg-indigo-600 text-white px-5 py-3 rounded-lg shadow hover:bg-indigo-700"
        >
          <Plus size={20}/> Add Language
        </button>
      </div>


      {languages.length === 0 && (
        <div className="flex flex-col items-center justify-center bg-white p-20 rounded-xl shadow text-gray-500">
          <Globe size={70} className="opacity-30"/>
          <h2 className="text-xl font-semibold mt-4">No Languages Yet</h2>
          <p className="text-sm">Add your first language to get started</p>
        </div>
      )}

      {languages.length > 0 && (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 w-full">

          {languages.map(l => (
            <div key={l.id} className="bg-white shadow rounded-xl p-6 border border-indigo-200">
              <h2 className="text-lg font-bold text-gray-900">{l.code.toUpperCase()}</h2>

              <div className="flex gap-2 pt-4">
                <button className="flex-1 bg-gray-100 hover:bg-gray-200 p-2 rounded"
                  onClick={() => startEdit(l)}
                >
                  <Edit2 size={16}/> Edit
                </button>

                <button className="flex-1 bg-red-100 text-red-700 hover:bg-red-200 p-2 rounded"
                  onClick={() => remove(l.id)}
                >
                  <Trash2 size={16}/> Delete
                </button>
              </div>
            </div>
          ))}

        </div>
      )}

      {/* Modal */}
      {openModal && (
        <div className="fixed inset-0 bg-black/40 flex justify-center items-center p-4">
          <div className="bg-white p-6 rounded-xl w-full max-w-md shadow">

            <h2 className="text-xl font-bold mb-4">{edit ? "Edit Language" : "Add Language"}</h2>

            <form onSubmit={submit} className="space-y-3">
              <input
                required
                placeholder="Language code (ex: en)"
                value={code}
                onChange={(e)=>setCode(e.target.value)}
                className="border p-2 w-full rounded"
              />

              <button className="bg-indigo-600 text-white w-full py-2 rounded-lg hover:bg-indigo-700">
                {edit ? "Update" : "Create"}
              </button>

              <button
                type="button"
                onClick={()=>setOpenModal(false)}
                className="w-full py-2 mt-1 rounded text-sm text-gray-500 hover:text-gray-800"
              >
                Cancel
              </button>
            </form>

          </div>
        </div>

      )}

    </div>
  );
}
