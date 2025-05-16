import { instance } from "./api-instance-provider";


export async function getAllUser() {
    try {
      const token = localStorage.getItem('userToken');
      const response = await instance.get(`/Auth`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      if (response.data.code === 200) {
        return response.data.data.users;  // Return the users array from the response
      }
      throw new Error(response.data.message || `Failed to fetch users`);
    } catch (error) {
      console.error(`Error getting users:`, error);
      throw error;
    }
  }
  
export async function updateStatus(id, isBanned) {
  try {
    const token = localStorage.getItem('userToken');
    const response = await instance.put(`/Auth/status`, {
      id: id,
      isBanned: isBanned
    }, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
    
    if (response.data.code === 200) {
      return response.data;
    }
    throw new Error(response.data.message || 'Failed to update account status');
  } catch (error) {
    console.error('Error updating account status:', error);
    throw error;
  }
}
  
  
export async function updateRole(id, role, commissionRate = 0) {
  try {
    const token = localStorage.getItem('userToken');
    const response = await instance.put(`/Auth/role`, {
      id: id,
      role: role,
      commissionRate: commissionRate
    }, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
    if (response.data.code === 200) {
      return response.data;
    }
    throw new Error(response.data.message || 'Failed to update role');
  } catch (error) {
    console.error('Error updating role:', error);
    throw error;
  }
}
