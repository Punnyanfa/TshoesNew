import { instance } from "./api-instance-provider";



export async function getAllRating() {
  try {
    const response = await instance.get(`/Rating`);
    if (response.data.code === 200) {
      return response.data.data;  // Trả về data của sản phẩm cụ thể
    }
    throw new Error(response.data.message || `Failed to fetch rating`);
  } catch (error) {
    console.error(`Error getting rating:`, error);
    throw error;
  }
}

export async function postRating(ratingData) {
  console.log(ratingData);
  try {
    const response = await instance.post(`/Rating`, ratingData);

    // ✅ THAY đổi điều kiện kiểm tra thành status HTTP
    if (response.status === 200) {
      return response.data;
    }

    // fallback nếu status != 200
    throw new Error(response.data?.message || `Failed to post rating`);
  } catch (error) {
    console.error(`Error posting rating:`, error);
    throw error;
  }
}