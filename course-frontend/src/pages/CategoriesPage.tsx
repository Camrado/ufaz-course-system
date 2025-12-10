import { useEffect, useState } from "react";
import { Tag, Plus, Edit2, Trash2, ArrowRight } from "lucide-react";
import { getAllCategories, createCategory, updateCategory, deleteCategory } from "../services/categoryService";
import type { Category } from "../types/Category";

export default function CategoriesPage({ onCategorySelect }: { onCategorySelect: (c: Category) => void }) {

  const [categories, setCategories] = useState<Category[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [edit, setEdit] = useState<Category | null>(null);
  const [name, setName] = useState("");

  const load = async () => {
    try {
      const res = await getAllCategories();
      setCategories(res.categories);
    } catch {
      console.log("Failed to load");
    }
  };

  useEffect(()=>{ load(); },[]);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    if(edit) await updateCategory({ categoryId: edit.id, name });
    else await createCategory({ name });

    setOpenModal(false);
    load();
  };

  const removeCategory = async (id: number) => {
  try {
    await deleteCategory(id);    // no confirmation, performs instantly
    await load();
  } catch (err) {
    console.error(err);
    alert("Cannot delete — maybe this category still contains courses.");
  }
};


  return (
    <div className="min-h-screen bg-gradient-to-b from-emerald-50 to-green-100 p-10">

      {/* Header */}
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="flex items-center gap-3 text-4xl font-bold text-gray-800">
            <Tag size={38} className="text-green-700"/>
            Categories
          </h1>
          <p className="text-gray-600 text-sm">Click a category to view its courses</p>
        </div>

        <button
          onClick={()=>{ setOpenModal(true); setEdit(null); setName(""); }}
          className="flex items-center gap-2 bg-green-600 text-white px-5 py-3 rounded-lg shadow hover:bg-green-700"
        >
          <Plus size={20}/> Add Category
        </button>
      </div>


      {/* LIST VIEW */}
      <div className="bg-white rounded-xl shadow divide-y border overflow-hidden">
        {categories.map(c => (
          <div key={c.id} className="flex items-center justify-between px-6 py-7 hover:bg-green-50 cursor-pointer">

            {/* LEFT — NAME, CLICK TO OPEN */}
            <div className="flex items-center gap-3" onClick={()=>onCategorySelect(c)}>
              <ArrowRight size={18} className="text-green-600"/>
              <span className="font-semibold text-gray-800 text-lg">{c.name}</span>
            </div>

            {/* RIGHT — ACTION BUTTONS */}
            <div className="flex gap-3">

              <button
                onClick={(e)=>{ e.stopPropagation(); setEdit(c); setName(c.name); setOpenModal(true); }}
                className="bg-gray-100 hover:bg-gray-200 px-3 py-1 rounded flex items-center gap-1"
              >
                <Edit2 size={16}/> Edit
              </button>

              <button
                onClick={(e)=>{ e.stopPropagation(); removeCategory(c.id); }}
                className="bg-red-100 text-red-700 hover:bg-red-200 px-3 py-1 rounded flex items-center gap-1"
              >
                <Trash2 size={16}/> Delete
              </button>

            </div>
          </div>
        ))}

        {categories.length === 0 && (
          <div className="text-center text-gray-500 py-10">
            No categories yet…
          </div>
        )}
      </div>



      {/* Modal */}
      {openModal && (
        <div className="fixed inset-0 bg-black/40 flex justify-center items-center p-4">
          <div className="bg-white p-6 rounded-xl w-full max-w-md shadow">
            <h2 className="text-xl font-bold mb-4">{edit ? "Edit Category" : "Add Category"}</h2>

            <form onSubmit={submit} className="space-y-3">
              <input
                required
                placeholder="Category name"
                value={name}
                onChange={(e)=>setName(e.target.value)}
                className="border p-2 w-full rounded"
              />

              <button className="bg-green-600 text-white w-full py-2 rounded-lg hover:bg-green-700">
                {edit ? "Update" : "Create"}
              </button>

              <button
                type="button"
                onClick={()=>setOpenModal(false)}
                className="w-full py-2 mt-1 rounded text-sm text-gray-500 hover:text-gray-700"
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
