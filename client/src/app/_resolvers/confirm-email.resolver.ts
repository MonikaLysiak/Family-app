import { ResolveFn } from '@angular/router';

export const confirmEmailResolver: ResolveFn<boolean> = (route, state) => {
  return true;
};

// export const confirmEmailResolver: ResolveFn<boolean> = (route, state) => {
//   const accountService = inject(AccountService);
//   return accountService.confirmEmail(route.queryParams);
// };
