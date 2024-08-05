import { ResolveFn } from '@angular/router';
import { UserFamiliesService } from '../_services/user-families.service';
import { inject } from '@angular/core';
import { FamilyMember } from '../_models/family-member';

export const familyMemberDetailedResolver: ResolveFn<FamilyMember> = (route, state) => {
  const familyService = inject(UserFamiliesService);

  return familyService.getFamilyMemberDetails(route.paramMap.get('familyMemberId')!);
};
