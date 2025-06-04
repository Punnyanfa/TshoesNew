<template>
  <Header />
  <section class="withdraw-modern-bg">
    <h2 class="withdraw-title">Withdraw Funds</h2>
    <div class="withdraw-balance-row">
      <span class="withdraw-balance-label">Current Balance:</span>
      <span class="withdraw-balance-amount">{{ balance.toLocaleString('vi-VN') }} ₫</span>
    </div>
    <form @submit.prevent="handleSubmit" class="withdraw-form-modern">
      <div class="withdraw-form-row">
        <label for="amount">Amount</label>
        <div class="withdraw-input-group">
          <span class="withdraw-currency">₫</span>
          <input
            type="number"
            id="amount"
            v-model="amount"
            placeholder="Enter amount to withdraw"
            min="0"
            step="1000"
            required
          />
        </div>
        <div v-if="amountError" class="withdraw-error">{{ amountError }}</div>
      </div>
      <div class="withdraw-form-row">
        <label for="email">Email Address</label>
        <input
          type="email"
          id="email"
          v-model="email"
          placeholder="Enter your email address"
          required
        />
      </div>
      <div class="withdraw-form-row">
        <label for="accountNumber">Account Number</label>
        <input
          type="text"
          id="accountNumber"
          v-model="accountNumber"
          placeholder="Enter your account number"
          required
        />
      </div>
      <div class="withdraw-form-row">
        <label for="accountName">Account Holder Name</label>
        <input
          type="text"
          id="accountName"
          v-model="accountName"
          placeholder="Enter account holder name"
          required
        />
      </div>
      <button type="submit" class="withdraw-btn-modern" :disabled="isSubmitting">
        <span v-if="isSubmitting"><i class="fas fa-spinner fa-spin"></i> Processing...</span>
        <span v-else>Withdraw Funds</span>
      </button>
    </form>
  </section>
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
/* Modern Withdraw Page Styles */
.withdraw-modern-bg {
  min-height: 100vh;
  padding: 48px 0 32px 0;
  background: linear-gradient(120deg, #f8fafc 0%, #e0e7ff 100%);
  display: flex;
  flex-direction: column;
  align-items: center;
}
.withdraw-title {
  font-size: 2.2rem;
  font-weight: 700;
  color: #4f46e5;
  margin-bottom: 32px;
  letter-spacing: 1px;
  text-align: center;
}
.withdraw-balance-row {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 1.2rem;
  color: #374151;
  margin-bottom: 32px;
  background: #eef2ff;
  padding: 16px 32px;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(79, 70, 229, 0.06);
}
.withdraw-balance-label {
  font-weight: 500;
  color: #6366f1;
}
.withdraw-balance-amount {
  font-weight: 700;
  color: #1e293b;
  font-size: 1.5rem;
}
.withdraw-form-modern {
  width: 100%;
  max-width: 480px;
  display: flex;
  flex-direction: column;
  gap: 22px;
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 24px rgba(79, 70, 229, 0.08);
  padding: 36px 32px 28px 32px;
}
.withdraw-form-row {
  display: flex;
  flex-direction: column;
  gap: 8px;
}
.withdraw-form-row label {
  font-size: 1rem;
  color: #4f46e5;
  font-weight: 600;
}
.withdraw-form-row input {
  padding: 12px 14px;
  border: 1px solid #e3e8ef;
  border-radius: 8px;
  font-size: 1rem;
  background: #f8fafc;
  transition: border 0.2s;
}
.withdraw-form-row input:focus {
  border-color: #4f46e5;
  outline: none;
  background: #fff;
}
.withdraw-input-group {
  display: flex;
  align-items: center;
}
.withdraw-currency {
  background: #eef2ff;
  color: #6366f1;
  padding: 12px 16px;
  border-radius: 8px 0 0 8px;
  font-weight: 600;
  font-size: 1rem;
  border: 1px solid #e3e8ef;
  border-right: none;
}
.withdraw-input-group input {
  border-radius: 0 8px 8px 0;
  border-left: none;
}
.withdraw-error {
  color: #dc2626;
  font-size: 14px;
  margin-top: 4px;
}
.withdraw-btn-modern {
  width: 100%;
  padding: 14px;
  background: linear-gradient(135deg, #6366f1, #4f46e5);
  color: #fff;
  font-size: 1.1rem;
  font-weight: 600;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.2s;
  margin-top: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.withdraw-btn-modern:disabled {
  background: #a5b4fc;
  cursor: not-allowed;
}
@media (max-width: 600px) {
  .withdraw-form-modern {
    padding: 18px 8px 16px 8px;
    max-width: 98vw;
  }
  .withdraw-balance-row {
    padding: 10px 8px;
    font-size: 1rem;
  }
  .withdraw-title {
    font-size: 1.3rem;
  }
}
</style>
