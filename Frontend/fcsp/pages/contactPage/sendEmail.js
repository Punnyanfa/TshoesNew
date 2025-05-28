import { sendEmail } from '../../server/auth/senEmail-service';

export async function sendEmail(userId, subject, body) {
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