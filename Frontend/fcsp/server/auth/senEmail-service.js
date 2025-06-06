import { instance } from "../api-instance-provider";

export async function sendEmail({ userId, subject, body, isHtml }) {
  console.log("userId", userId);
  console.log("subject", subject);
  console.log("body", body);
  console.log("isHtml", isHtml);
  try {
    const response = await instance.post("/Auth/SendEmailToAdmin", {
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