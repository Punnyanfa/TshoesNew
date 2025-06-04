<template>
  <Header />
  <div class="container mt-5">
    <h2 class="text-center mb-4">Withdrawal Page</h2>
    
    <!-- Balance Display -->
    <div class="balance-container mb-4">
      <div class="balance-label">Current Balance</div>
      <div class="balance-amount">{{ balance.toFixed(2) }} ₫</div>
    </div>

    <form @submit.prevent="handleSubmit" class="form-container">
      <div class="mb-4">
        <label for="amount" class="form-label">Withdrawal Amount</label>
        <div class="input-group">
          <span class="input-group-text">₫</span>
          <input
            type="number"
            id="amount"
            v-model="amount"
            class="form-control"
            placeholder="Enter amount to withdraw"
            min="0"
            step="0.01"
            required
          />
        </div>
        <div v-if="amountError" class="error-message">{{ amountError }}</div>
      </div>

      <div class="mb-4">
        <label for="email" class="form-label">Email Address</label>
        <input
          type="email"
          id="email"
          v-model="email"
          class="form-control"
          placeholder="Enter your email address"
          required
        />
      </div>

      <div class="mb-4">
        <label for="accountNumber" class="form-label">Account Number</label>
        <input
          type="text"
          id="accountNumber"
          v-model="accountNumber"
          class="form-control"
          placeholder="Enter your account number"
          required
        />
      </div>

      <div class="mb-4">
        <label for="accountName" class="form-label">Account Holder Name</label>
        <input
          type="text"
          id="accountName"
          v-model="accountName"
          class="form-control"
          placeholder="Enter account holder name"
          required
        />
      </div>

      <button type="submit" class="btn btn-submit w-100" :disabled="isSubmitting">
        {{ isSubmitting ? 'Processing...' : 'Withdraw Funds' }}
      </button>
    </form>
  </div>
</template>

<script>
import { getBalance } from '~/server/balance-service';

export default {
  data() {
    return {
      email: "",
      accountNumber: "",
      accountName: "",
      amount: "",
      balance: 0,
      amountError: "",
      isSubmitting: false,
    };
  },
  async created() {
    try {
      // Assuming you have user ID stored in localStorage or Vuex
      const userId = localStorage.getItem('userId'); // Adjust based on your auth implementation
      console.log('Fetching balance for userId:', userId);
      
      if (userId) {
        const balanceData = await getBalance(userId);
        console.log('Balance API Response:', balanceData);
        this.balance = balanceData.data?.balance || 0;
        console.log('Current balance set to:', this.balance);
      } else {
        console.warn('No userId found in localStorage');
      }
    } catch (error) {
      console.error('Error fetching balance:', error);
      console.error('Error details:', {
        message: error.message,
        status: error.response?.status,
        data: error.response?.data
      });
    }
  },
  watch: {
    amount(newValue) {
      this.validateAmount(newValue);
    }
  },
  methods: {
    validateAmount(value) {
      const amount = parseFloat(value);
      this.amountError = "";
      console.log('Validating amount:', amount, 'Current balance:', this.balance);

      if (isNaN(amount)) {
        this.amountError = "Please enter a valid amount";
        console.log('Validation failed: Invalid amount format');
        return false;
      }

      if (amount <= 0) {
        this.amountError = "Amount must be greater than 0";
        console.log('Validation failed: Amount <= 0');
        return false;
      }

      if (amount > this.balance) {
        this.amountError = "Insufficient balance";
        console.log('Validation failed: Amount exceeds balance', {
          requested: amount,
          available: this.balance
        });
        return false;
      }

      // Assuming a minimum withdrawal of 10,000 VND for this example, adjust as needed.
      const minWithdrawal = 10000;
      if (amount < minWithdrawal) {
        this.amountError = `Minimum withdrawal amount is ${minWithdrawal.toLocaleString('vi-VN')} ₫`;
        console.log('Validation failed: Amount below minimum');
        return false;
      }

      console.log('Amount validation passed');
      return true;
    },
    async handleSubmit() {
      console.log('Starting withdrawal process...');
      console.log('Form data:', {
        amount: this.amount,
        email: this.email,
        accountNumber: this.accountNumber,
        accountName: this.accountName
      });

      if (!this.validateAmount(this.amount)) {
        console.log('Withdrawal cancelled: Amount validation failed');
        return;
      }

      this.isSubmitting = true;
      try {
        // Here you would typically call your withdrawal API
        const withdrawalData = {
          email: this.email,
          accountNumber: this.accountNumber,
          accountName: this.accountName,
          amount: parseFloat(this.amount),
          timestamp: new Date().toISOString()
        };
        
        console.log('Submitting withdrawal request:', withdrawalData);
        
        // Update balance after successful withdrawal
        const oldBalance = this.balance;
        this.balance -= parseFloat(this.amount);
        console.log('Balance updated:', {
          oldBalance,
          withdrawalAmount: this.amount,
          newBalance: this.balance
        });

        alert("Withdrawal request submitted successfully!");
        
        // Reset form
        this.email = "";
        this.accountNumber = "";
        this.accountName = "";
        this.amount = "";
        console.log('Form reset completed');
      } catch (error) {
        console.error('Withdrawal error:', error);
        console.error('Error details:', {
          message: error.message,
          status: error.response?.status,
          data: error.response?.data,
          timestamp: new Date().toISOString()
        });
        alert("Failed to process withdrawal. Please try again.");
      } finally {
        this.isSubmitting = false;
        console.log('Withdrawal process completed');
      }
    },
  },
};
</script>

<style scoped>
.container {
  max-width: 550px;
  background: linear-gradient(145deg, #ffffff, #f8f9fa);
  padding: 35px;
  border-radius: 20px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
  margin: 60px auto;
}

h2 {
  font-family: 'Poppins', sans-serif;
  color: #2c3e50;
  font-size: 28px;
  font-weight: 600;
  margin-bottom: 30px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.form-container {
  background-color: white;
  padding: 30px;
  border-radius: 15px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
}

.form-label {
  font-weight: 500;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 15px;
}

.form-control {
  border-radius: 12px;
  border: 2px solid #e9ecef;
  padding: 12px 16px;
  font-size: 15px;
  transition: all 0.3s ease;
  background-color: #f8f9fa;
}

.form-control:focus {
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.1);
  background-color: #ffffff;
}

.form-control::placeholder {
  color: #adb5bd;
}

.btn-submit {
  background: linear-gradient(135deg, #4a90e2, #357abd);
  color: white;
  font-size: 16px;
  font-weight: 600;
  padding: 14px;
  border: none;
  border-radius: 12px;
  transition: all 0.3s ease;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-top: 10px;
}

.btn-submit:hover {
  background: linear-gradient(135deg, #357abd, #2c5f9e);
  transform: translateY(-1px);
  box-shadow: 0 4px 15px rgba(74, 144, 226, 0.2);
}

.btn-submit:active {
  transform: translateY(0);
}

.mb-4 {
  margin-bottom: 1.8rem;
}

@media (max-width: 576px) {
  .container {
    margin: 30px 15px;
    padding: 20px;
  }
  
  .form-container {
    padding: 20px;
  }
}

.balance-container {
  background: linear-gradient(135deg, #4a90e2, #357abd);
  color: white;
  padding: 20px;
  border-radius: 15px;
  text-align: center;
  margin-bottom: 30px;
  box-shadow: 0 4px 15px rgba(74, 144, 226, 0.2);
}

.balance-label {
  font-size: 16px;
  font-weight: 500;
  margin-bottom: 8px;
  opacity: 0.9;
}

.balance-amount {
  font-size: 32px;
  font-weight: 600;
  letter-spacing: 0.5px;
}

.input-group {
  display: flex;
  align-items: center;
}

.input-group-text {
  background-color: #f8f9fa;
  border: 2px solid #e9ecef;
  border-right: none;
  border-radius: 12px 0 0 12px;
  padding: 12px 16px;
  color: #2c3e50;
  font-weight: 500;
}

.input-group .form-control {
  border-radius: 0 12px 12px 0;
}

.error-message {
  color: #dc3545;
  font-size: 14px;
  margin-top: 5px;
}

.btn-submit:disabled {
  background: #ccc;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}
</style>
