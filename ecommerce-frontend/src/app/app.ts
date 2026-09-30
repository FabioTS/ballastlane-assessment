import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

type Product = {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
};

type AuthSession = {
  token: string;
  name: string;
  email: string;
  role: string;
};

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styles: `
    :host { display: block; font-family: Arial, sans-serif; background: #f4f7fb; min-height: 100vh; padding: 32px; color: #1f2937; }
    .container { max-width: 1200px; margin: 0 auto; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    .grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 24px; }
    .panel { background: white; border-radius: 14px; padding: 20px; box-shadow: 0 8px 18px rgba(15, 23, 42, 0.08); }
    h1, h2, h3 { margin-top: 0; }
    form { display: flex; flex-direction: column; gap: 12px; }
    input, textarea, select, button { border-radius: 8px; border: 1px solid #d9e2ec; padding: 10px 12px; font-size: 14px; }
    button { cursor: pointer; background: #1d4ed8; color: white; font-weight: 600; border: none; }
    button.secondary { background: #e2e8f0; color: #111827; }
    button.danger { background: #dc2626; }
    .products table { width: 100%; border-collapse: collapse; }
    .products th, .products td { padding: 10px 12px; border-bottom: 1px solid #e5e7eb; text-align: left; }
    .badge { display: inline-block; padding: 6px 10px; border-radius: 999px; background: #dbeafe; color: #1d4ed8; font-size: 12px; font-weight: 600; }
    .status { margin-top: 12px; color: #374151; }
    .error { color: #b91c1c; font-weight: 600; }
    .success { color: #166534; font-weight: 600; }
    .muted { color: #6b7280; }
    .toolbar { display: flex; gap: 12px; flex-wrap: wrap; }
  `,
  template: `
    <div class="container">
      <div class="header">
        <div>
          <p class="muted">E-Commerce Catalog</p>
          <h1>Product Portal</h1>
        </div>
        <div class="toolbar" *ngIf="session(); else guestStatus">
          <span class="badge">{{ session()?.role }}</span>
          <button class="secondary" type="button" (click)="logout()">Logout</button>
        </div>
        <ng-template #guestStatus>
          <span class="badge">Guest</span>
        </ng-template>
      </div>

      <div class="grid">
        <div class="panel" *ngIf="!session()">
          <h2>Login</h2>
          <form (ngSubmit)="login()">
            <input [(ngModel)]="loginModel.email" name="email" type="email" placeholder="Email" required />
            <input [(ngModel)]="loginModel.password" name="password" type="password" placeholder="Password" required />
            <button type="submit">Login</button>
          </form>
        </div>

        <div class="panel" *ngIf="!session()">
          <h2>Register</h2>
          <form (ngSubmit)="register()">
            <input [(ngModel)]="registerModel.name" name="registerName" placeholder="Full name" required />
            <input [(ngModel)]="registerModel.email" name="registerEmail" type="email" placeholder="Email" required />
            <input [(ngModel)]="registerModel.password" name="registerPassword" type="password" placeholder="Password" required />
            <select [(ngModel)]="registerModel.role" name="registerRole">
              <option value="User">User</option>
              <option value="Admin">Admin</option>
            </select>
            <button type="submit">Create account</button>
          </form>
        </div>

        <div class="panel" *ngIf="session() && isAdmin()">
          <h2>{{ editingId ? 'Edit product' : 'Create product' }}</h2>
          <form (ngSubmit)="saveProduct()">
            <input [(ngModel)]="productForm.name" name="productName" placeholder="Name" required />
            <textarea [(ngModel)]="productForm.description" name="productDescription" placeholder="Description" rows="3" required></textarea>
            <input [(ngModel)]="productForm.price" name="productPrice" type="number" min="0.01" step="0.01" placeholder="Price" required />
            <input [(ngModel)]="productForm.stockQuantity" name="productStock" type="number" min="0" placeholder="Stock" required />
            <div class="toolbar">
              <button type="submit">{{ editingId ? 'Save changes' : 'Create' }}</button>
              <button type="button" class="secondary" (click)="resetForm()">Reset</button>
            </div>
          </form>
        </div>
      </div>

      <div class="panel products" style="margin-top: 24px;">
        <h2>Products</h2>
        <p class="status" *ngIf="statusMessage">{{ statusMessage }}</p>
        <p class="error" *ngIf="errorMessage">{{ errorMessage }}</p>
        <p class="success" *ngIf="successMessage">{{ successMessage }}</p>

        <table *ngIf="products().length; else emptyState">
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th>Price</th>
              <th>Stock</th>
              <th *ngIf="isAdmin()">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let product of products()">
              <td>{{ product.name }}</td>
              <td>{{ product.description }}</td>
              <td>{{ product.price | currency }}</td>
              <td>{{ product.stockQuantity }}</td>
              <td *ngIf="isAdmin()">
                <div class="toolbar">
                  <button type="button" class="secondary" (click)="startEdit(product)">Edit</button>
                  <button type="button" class="danger" (click)="deleteProduct(product.id)">Delete</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>

        <ng-template #emptyState>
          <p class="muted">No products available.</p>
        </ng-template>
      </div>
    </div>
  `,
})
export class App implements OnInit {
  private readonly apiUrl = 'https://localhost:7151/api';
  protected readonly products = signal<Product[]>([]);
  protected readonly session = signal<AuthSession | null>(this.readSession());
  protected readonly loginModel = { email: 'admin@ecommerce.local', password: 'Admin123!' };
  protected readonly registerModel = { name: '', email: '', password: '', role: 'User' };
  protected readonly productForm = {
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
  };
  protected editingId: string | null = null;
  protected errorMessage = '';
  protected successMessage = '';
  protected statusMessage = 'Use the demo credentials to log in and manage products.';

  constructor(private readonly http: HttpClient) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  protected isAdmin(): boolean {
    return this.session()?.role === 'Admin';
  }

  protected login(): void {
    this.clearAlerts();
    this.http.post<AuthSession>(`${this.apiUrl}/auth/login`, this.loginModel).subscribe({
      next: (response) => {
        this.persistSession(response);
        this.statusMessage = `Logged in as ${response.name}.`;
      },
      error: () => {
        this.errorMessage = 'Login failed. Please check your credentials.';
      },
    });
  }

  protected register(): void {
    this.clearAlerts();
    this.http.post<AuthSession>(`${this.apiUrl}/auth/register`, this.registerModel).subscribe({
      next: (response) => {
        this.persistSession(response);
        this.successMessage = 'Account created successfully.';
      },
      error: () => {
        this.errorMessage = 'Registration failed. Please try a different email.';
      },
    });
  }

  protected logout(): void {
    localStorage.removeItem('ecommerce-session');
    this.session.set(null);
    this.successMessage = 'Logged out successfully.';
  }

  protected loadProducts(): void {
    const headers = this.authHeaders();
    this.http.get<Product[]>(`${this.apiUrl}/products`, { headers }).subscribe({
      next: (products) => this.products.set(products),
      error: () => this.errorMessage = 'Unable to load products. Ensure the API is running.',
    });
  }

  protected startEdit(product: Product): void {
    this.editingId = product.id;
    this.productForm.name = product.name;
    this.productForm.description = product.description;
    this.productForm.price = product.price;
    this.productForm.stockQuantity = product.stockQuantity;
  }

  protected resetForm(): void {
    this.editingId = null;
    this.productForm.name = '';
    this.productForm.description = '';
    this.productForm.price = 0;
    this.productForm.stockQuantity = 0;
  }

  protected saveProduct(): void {
    if (!this.session()) {
      this.errorMessage = 'You must be logged in to manage products.';
      return;
    }

    const payload = {
      name: this.productForm.name,
      description: this.productForm.description,
      price: Number(this.productForm.price),
      stockQuantity: Number(this.productForm.stockQuantity),
    };

    const headers = this.authHeaders();
    const request = this.editingId
      ? this.http.put<Product>(`${this.apiUrl}/products/${this.editingId}`, payload, { headers })
      : this.http.post<Product>(`${this.apiUrl}/products`, payload, { headers });

    request.subscribe({
      next: () => {
        this.successMessage = this.editingId ? 'Product updated successfully.' : 'Product created successfully.';
        this.resetForm();
        this.loadProducts();
      },
      error: () => {
        this.errorMessage = 'Product validation failed. Check the values and try again.';
      },
    });
  }

  protected deleteProduct(productId: string): void {
    if (!this.session()) {
      this.errorMessage = 'You must be logged in to delete products.';
      return;
    }

    this.http.delete(`${this.apiUrl}/products/${productId}`, { headers: this.authHeaders() }).subscribe({
      next: () => {
        this.successMessage = 'Product deleted.';
        this.loadProducts();
      },
      error: () => {
        this.errorMessage = 'Delete failed.';
      },
    });
  }

  private authHeaders(): HttpHeaders {
    const token = this.session()?.token;
    return token ? new HttpHeaders({ Authorization: `Bearer ${token}` }) : new HttpHeaders();
  }

  private persistSession(session: AuthSession): void {
    localStorage.setItem('ecommerce-session', JSON.stringify(session));
    this.session.set(session);
    this.statusMessage = `Welcome, ${session.name}.`;
  }

  private readSession(): AuthSession | null {
    const raw = localStorage.getItem('ecommerce-session');
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      return null;
    }
  }

  private clearAlerts(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.statusMessage = '';
  }
}
