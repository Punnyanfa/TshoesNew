import { sendEmail } from '../../server/auth/senEmail-service';

export async function sendEmailToUser(userId, subject, body) {
  return axios.post(
    "https://fcspwebapi20250527114117.azurewebsites.net/api/Auth/SendEmailToUser",
    {
      userId,
      subject,
      body,
      isHtml: true,
    }
  );
} 