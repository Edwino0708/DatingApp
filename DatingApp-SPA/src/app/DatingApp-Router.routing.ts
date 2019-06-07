import { Routes, RouterModule } from '@angular/router';
import { ValueComponent } from './value/value.component';

const routes: Routes = [
  
    { path: 'h', component: ValueComponent }
];

export const DatingAppRouterRoutes = RouterModule.forChild(routes);
