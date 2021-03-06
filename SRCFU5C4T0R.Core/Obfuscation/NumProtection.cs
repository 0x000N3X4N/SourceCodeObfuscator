﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SRCFU5C4T0R.Core.Kernel.API;
using srclib.Helpers;

namespace SRCFU5C4T0R.Core.Obfuscation {
/// <summary>
/// This class provide to mutation expressions
/// </summary>
class NumProtection {
  private FieldDeclarationSyntax createNewField(FieldDeclarationSyntax field, VariableDeclaratorSyntax fld) {
    double src_initializer = Convert.ToDouble(field.Declaration.Variables.Last().Initializer.Value.ToString());
    string encrypted_arg =
        Convert.ToString(a1f95c1bb38e78c13308
            .__0xᵉ7ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ362ᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ(
                src_initializer));
    FieldDeclarationSyntax tmp = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration( 
                                   SyntaxFactory.ParseTypeName(field.Declaration.Type.ToString()), 
                                     SyntaxFactory.SeparatedList(new[] {
                                       SyntaxFactory.VariableDeclarator(
                                         SyntaxFactory.Identifier(fld.Identifier.ValueText), null,
                                            SyntaxFactory.EqualsValueClause(SyntaxFactory.ParseExpression( (src_initializer % 1 > 0) ?
                                                $"a1f95c1bb38e78c13308.__0xᵉ1ᵉ333ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ364ᵈᵉᵃᵈᶜᵒᵈᵉᵈ" +
                                                    $"ᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ({ encrypted_arg })"
                                            : "a1f95c1bb38e78c13308.__0xᵉ1488ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ("
                                                + encrypted_arg + ")")
                                         )
                                        )
                                     })
                                 ));
    
    /******************* Adding modifiers (private, readonly, etc.) *****************/
    if (field.Modifiers.Count > 0)
      foreach (var modifer in field.Modifiers)
        tmp = tmp.AddModifiers(modifer);
    /*******************************************************************************/
    return tmp;
  }

  public Solution confuseExpressions(Solution sln) {
    var projId = APIAnalyze.projId;
    MSBuildWorkspace wspLoading = MSBuildWorkspace.Create();
    var prjLoading = wspLoading.OpenProjectAsync(Config.pathToOriginal).Result;
    Solution slnLoading = wspLoading.CurrentSolution;
    foreach (var prjId in slnLoading.GetProjectDependencyGraph().GetTopologicallySortedProjects()) {
      var prj = slnLoading.GetProject(prjId);
      foreach (var doc in prj.Documents)
        sln = sln.AddDocument(DocumentId.CreateNewId(projId), doc.Name, doc.GetTextAsync().Result);
    }

    var documents = sln.Projects.SelectMany(x => x.Documents).Select(x => x.Id).ToList();
    Console.WriteLine($"Count of documents founded is: { documents.Count }");
    foreach (var documentId in documents) {
      List<LocalDeclarationStatementSyntax> vars;
      List<FieldDeclarationSyntax> fields;
      List<AssignmentExpressionSyntax> expr; //TODO: expressions
      
      Console.WriteLine($"Current document name is: { sln.GetDocument(documentId).Name }");
      var current_doc = sln.GetDocument(documentId);
      var editor = DocumentEditor.CreateAsync(current_doc).Result;
      var syntax = current_doc.GetSyntaxRootAsync().Result;
      vars = syntax.DescendantNodes().OfType<LocalDeclarationStatementSyntax>()
            .Where(x => x.Declaration.Variables.Last().Initializer != null && 
                        x.Declaration.Variables.Last().Initializer.Value.Kind() == SyntaxKind.NumericLiteralExpression)
            .ToList();

      fields = syntax.DescendantNodes().OfType<FieldDeclarationSyntax>()
             .Where(x => x.Declaration.Variables.Last().Initializer != null && 
                         x.Declaration.Variables.Last().Initializer.Value.Kind() == SyntaxKind.NumericLiteralExpression)
             .ToList();

      expr = syntax.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();

      if (expr.Count > 0) {
        Console.WriteLine($"[MUTATION] : Count of founded expressions is: {expr.Count}");
        foreach (var current_expr in expr) {
          foreach (var expr_node in current_expr.Right.ChildNodes()) {
            if (expr_node.Kind() == SyntaxKind.NumericLiteralExpression) {
              double src_token = Convert.ToDouble(expr_node.ToString());
              double encrypted_arg = a1f95c1bb38e78c13308
                   .__0xᵉ7ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ362ᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ(src_token);
              string formatted_node = (src_token % 1 > 0)
                    ? "a1f95c1bb38e78c13308.__0xᵉ1ᵉ333ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ364ᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ("
                      + encrypted_arg + ")"
                    : "a1f95c1bb38e78c13308.__0xᵉ1488ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ("
                      + encrypted_arg + ")";
              var new_node = SyntaxFactory.ParseExpression($"{formatted_node}");
              editor.ReplaceNode(expr_node, new_node);
            }
          }
        }
      }

      foreach(var field in fields) {
        foreach(var fld in field.Declaration.Variables) {
          Console.WriteLine($"[MUTATION]: founded field: { field }");  
          FieldDeclarationSyntax newField = createNewField(field, fld);
          Console.WriteLine($"[MUTATION]: new field is: { newField }");
          editor.ReplaceNode(fld, newField);
        }
        current_doc = editor.GetChangedDocument();
     }

     foreach(var var_declorator in vars) {
       foreach(var vr in var_declorator.Declaration.Variables) {
         if(vr.Initializer == null) continue;
           double src_initializer = Convert.ToDouble(vr.Initializer.Value.ToString());
           string encrypted_arg =
               Convert.ToString(a1f95c1bb38e78c13308
                   .__0xᵉ7ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ362ᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ(
                       src_initializer));
           string formattedInitializer = (src_initializer % 1 > 0) ?
               "a1f95c1bb38e78c13308.__0xᵉ1ᵉ333ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃ364ᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ(" 
                                         + encrypted_arg + ");"
               : "a1f95c1bb38e78c13308.__0xᵉ1488ᵉ371ᶠ3ᶠ8ᵃᵃ13212ᵉ9ᵃᵈ4790ᵃ53432ᶠ2ᵈᵇᶜᵃᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉᵈᵉᵃᵈᶜᵒᵈᵉ("
                                        + encrypted_arg + ");";

           var newVariable = SyntaxFactory.ParseStatement($"{ var_declorator.Declaration.Type } { vr.Identifier } = { formattedInitializer };");
           newVariable.NormalizeWhitespace();
           editor.ReplaceNode(vr, newVariable);
           Console.WriteLine($"[MUTATION] Key: { vr.Identifier } replaced with value: { formattedInitializer }");
         }
       current_doc = editor.GetChangedDocument();
     }
     sln = sln.RemoveDocument(documentId);
     sln = sln.AddDocument(DocumentId.CreateNewId(projId), current_doc.Name, current_doc.GetTextAsync().Result);
    }
    return sln;
  }
}
}
