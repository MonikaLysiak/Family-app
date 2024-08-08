import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './_guards/admin.guard';
import { UserFamiliesListComponent } from './user-families/user-families-list/user-families-list.component';
import { FamilyMemberListComponent } from './family/family-member-list/family-member-list.component';
import { FamilyListsComponent } from './family/family-lists/family-lists.component';
import { FamilyPhotosComponent } from './family/family-photos/family-photos.component';
import { FamilyHomeComponent } from './family/family-home/family-home.component';
import { FamilyMemberCardComponent } from './family-member/family-member-card/family-member-card.component';
import { familyMemberDetailedResolver } from './_resolvers/family-member-detailed.resolver';
import { FamilyMemberDetailsComponent } from './family-member/family-member-details/family-member-details.component';
import { familyDetailedResolver } from './_resolvers/family-detailed.resolver';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'families', component: UserFamiliesListComponent},
      {path: 'families/:familyId', component: FamilyHomeComponent, resolve: {family: familyDetailedResolver}},
      {path: 'familyMembers', component: FamilyMemberListComponent},
      {path: 'familyMembers/:familyMemberId', component: FamilyMemberDetailsComponent, resolve: {familyMember: familyMemberDetailedResolver}},
      {path: 'familyLists', component: FamilyListsComponent},
      {path: 'familyPhotos', component: FamilyPhotosComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: memberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard]},
      {path: 'lists', component: ListsComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard]},
      {path: 'members', component: MemberListComponent}
    ]
  },
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
