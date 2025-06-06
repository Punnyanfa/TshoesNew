import { instance } from "../api-instance-provider";

export async function resetPassword(email, newPassword) {
  try {
    const response = await instance.post("/Auth/reset-password", { email, newPassword });
    return response.data;
  } catch (error) {
    console.error("Error resetting password:", error);
    throw error;
  }
}