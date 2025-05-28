import { instance } from "../api-instance-provider";

export async function sendEmail(userId, subject, body) {
  try {
    const response = await instance.post("/Auth/SendEmailToUser", {
      userId,
      subject,
      body,
      isHtml: true,
    });
    return response.data;
  } catch (error) {
    console.error(`Error sending email:`, error);
    throw error;
  }
}   
export async function changePassword(email, currentPassword,newPassword) {
  try {
    const response = await instance.put(`/Auth/password/`, { email, currentPassword,newPassword });
    return response.data;
  } catch (error) {
    console.error(`Error changing password:`, error);
    throw error;
  }
}
export async function forgotPassword(email, purposeType, expiryTimeInMinutes) {
  console.log("forgotPassword", email, purposeType, expiryTimeInMinutes);
  try {
    const response = await instance.post(`/Auth/generate`, { email, purposeType, expiryTimeInMinutes });
    return response.data;
  } catch (error) {
    console.error(`Error forgot password:`, error);
    throw error;
  }
}