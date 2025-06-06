import { fr } from "element-plus/es/locale/index.mjs";
import { instance } from "./api-instance-provider";

export async function CustomShoeDesign(data) {
  console.log("data",data);
  let formData = new FormData();

  // Thêm các trường đơn giản
  formData.append('UserId', data.userId);
  formData.append('CustomShoeDesignTemplateId', data.templateId);
  formData.append('Name', data.name);
  formData.append('Description', data.description);
  formData.append('DesignerMarkup', data.DesignerMarkup);

  // DesignData: nếu là object, stringify rồi append như file
  if (typeof data.designData === 'object') {
    const blob = new Blob([JSON.stringify(data.designData)], { type: 'application/json' });
    formData.append('DesignData', blob);
  } else {
    formData.append('DesignData', data.designData);
  }

  // Preview Images: là mảng file
  if (Array.isArray(data.previewImages)) {
    data.previewImages.forEach((img) => {
      formData.append('CustomShoeDesignPreviewImages', img);
    });
  }

  // TextureIds: là mảng số
  if (Array.isArray(data.textureIds)) {
    data.textureIds.forEach(id => formData.append('TextureIds', id));
  }

  // ServiceIds: là mảng số
  if (Array.isArray(data.ServiceIds)) {
    data.ServiceIds.forEach(id => formData.append('ServiceIds', id));
  }

  // Gửi request
  const response = await instance.post(`/CustomShoeDesign`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  });
  return response.data;
}
export async function updateStatus(id, status) {
  try {
    const response = await instance.put(`/CustomShoeDesign/status`, { id, status });
    return response.data;
  } catch (error) {
    console.error(`Error updating status:`, error);
    throw error;
  }
}