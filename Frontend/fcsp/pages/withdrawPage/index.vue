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
            placeholder="Enter amount to withdraw (minimum 20,000 VND)"
            min="20000"
            step="1000"
            required
          />
        </div>
        <div v-if="amountError" class="withdraw-error">{{ amountError }}</div>
      </div>
    
      <div class="withdraw-form-row">
        <label for="bank">Bank Name</label>
        <select id="bank" v-model="selectedBank" required class="form-select" style="padding:12px 14px; border-radius:8px; background:#f8fafc; border:1px solid #e3e8ef;">
          <option disabled value="">Select your bank</option>
          <option v-for="bank in bankList" :key="bank" :value="bank">{{ bank }}</option>
        </select>
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
import { sendEmail } from '~/server/auth/senEmail-service';
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
      selectedBank: "",
      bankList: [
        'Vietcombank',
        'Techcombank',
        'BIDV',
        'VietinBank',
        'MB Bank',
        'ACB',
        'Sacombank',
        'TPBank',
        'VPBank',
        'SHB',
        'HDBank',
        'Eximbank',
        'OCB',
        'SeABank',
        'VIB',
        'MSB',
        'Nam A Bank',
        'LienVietPostBank',
        'ABBANK',
        'SCB',
        'PVcomBank',
        'Bac A Bank',
        'Saigonbank',
        'BaoVietBank',
      ],
    };
  },
  async created() {
    try {
      // Lấy email từ localStorage
      this.email = localStorage.getItem('userEmail') || '';
      // Lấy userId để lấy balance
      const userId = localStorage.getItem('userId');
      if (userId) {
        const balanceData = await getBalance(userId);
        this.balance = balanceData.data?.balance || 0;
      }
    } catch (error) {
      console.error('Error fetching balance:', error);
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

      const minWithdrawal = 20000;
      if (amount < minWithdrawal) {
        this.amountError = `Minimum withdrawal amount is ${minWithdrawal.toLocaleString('vi-VN')} ₫`;
        console.log('Validation failed: Amount below minimum');
        return false;
      }

      console.log('Amount validation passed');
      return true;
    },
    async handleSubmit() {
      if (!this.validateAmount(this.amount)) return;
      if (!this.selectedBank) {
        alert('Please select your bank.');
        return;
      }
      if (!this.accountNumber || this.accountNumber.length < 8) {
        alert('Please enter a valid account number (minimum 8 characters).');
        return;
      }
      if (!this.accountName || this.accountName.length < 3) {
        alert('Please enter a valid account holder name (minimum 3 characters).');
        return;
      }
      this.isSubmitting = true;
      try {
        const userId = localStorage.getItem('userId');
        const subject = `${this.email} rút tiền`;
        const bodyText = `- Bank: ${this.selectedBank}\n- Account Number: ${this.accountNumber}\n- Account Name: ${this.accountName}\n- Amount: ${parseFloat(this.amount).toLocaleString('vi-VN')}₫`;
        await sendEmail({ userId, subject, body: bodyText, isHtml: false });
        this.balance -= parseFloat(this.amount);
        alert("Withdrawal request submitted successfully!");
        // Reset form (trừ email)
        this.selectedBank = "";
        this.accountNumber = "";
        this.accountName = "";
        this.amount = "";
      } catch (error) {
        alert("Failed to process withdrawal. Please try again.");
      } finally {
        this.isSubmitting = false;
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
