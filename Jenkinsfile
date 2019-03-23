pipeline {
	agent any
	environment {
		// Very Important; stops from overwriting on temporary files
		HOME = '/tmp'

		// Global Variables
		ApplicationName = "Supernet-GoLang"

		// Color Coding
		JobStartCC = '#ffff00'
		SuccessJobCC = '#7cfc00'
		FailureJobCC = '#ff4500'
		UnstableJobCC = '#0000ff'

		// Slack Notification
		JobStartSN = "STARTED: Job has Started - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		JobSuccessSN = "SUCCESS: Job - ${currentBuild.fullDisplayName} has been completed. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		JobFailureSN = "FAILURE: Job - ${currentBuild.fullDisplayName} has failed. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		UnstableJobSN = "UNSTABLE: Job - ${currentBuild.fullDisplayName} is unstable. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"

		// Dotnet
			// Dotnet Bat Variables
			DotnetProjectName = "Mango.sln"
			DotnetProjectOptions = "/p:Configuration=Release /p:Platform=\"Any CPU\""
			DotnetProductVersion = "/p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

			// Dotnet Run Command Variables
			DotnetBuild = 'nuget restore'
			DotnetTest = 'dotnet test'

			// Dotnet Test
			DotnetTestResultDir = "-r bin/Release/netcoreapp2.0/UnitTest/${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}"

			// Dotnet Release
			DotnetReleaseOutputDir = "-o bin/Release/netcoreapp2.0/FDD/Supernet-${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}"
			DotnetReleaseFDD = "dotnet publish ${env.DotnetProjectName} -c Release -f netcoreapp2.0 ${env.DotnetReleaseOutputDir}"
			DotnetReleaseSCDWindows10 = "dotnet publish ${env.DotnetProjectName} -c Release -f netcoreapp2.0 -r win10-arm64 ${env.DotnetReleaseOutputDir}"
			DotnetReleaseSCDUbuntu16 = "dotnet publish ${env.DotnetProjectName} -c Release -f netcoreapp2.0 -r ubuntu.16.04-x64 ${env.DotnetReleaseOutputDir}"
	}

	stages {
		stage ('Build: Dotnet Project') {
			agent {
				docker { 
					image 'microsoft/dotnet'
				}
			}
			steps {
				slackSend channel: '#bangalore_dev_team',
					color: "${env.JobStartCC}",
					message:  "${env.JobStartSN}"
				dir ("mango") {
					sh "dotnet build ${env.DotnetProjectName}"
				}
			}
		}
		stage ('Testing: Nunit Testing') {
			agent {
				docker { 
					image 'microsoft/dotnet'
				}
			}
			steps {
				dir('mango') {
					sh "${env.DotnetTest} ${env.DotnetProjectName} ${env.DotnetTestResultDir}"
					nunit testResultsPattern: 'TestResult.xml'
				}
			}
		}
		
		// stage ('Publish: Dotnet Project FDD & SCD') {
		// 	agent {
		// 		docker { 
		// 			image 'microsoft/dotnet'
		// 		}
		// 	}
		// 	steps {
		// 		dir('mango') {
		// 			sh "${env.DotnetReleaseFDD}"
		// 			sh "${env.DotnetReleaseSCDWindows10}"
		// 			sh "${env.DotnetReleaseSCDUbuntu16}"
		// 			sh "tar -czvf dotnet-supernet.tar.gz bin/Release/netcoreapp2.0"
		// 			// rtUpload (
		// 			// 	serverId: "cs-artifactory",
		// 			// 	specPath: 'Artifactory/upload-spec.json',
		// 			// 	failNoOp: true
		// 			// )
		// 			sh "curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -T dotnet-supernet.tar.gz \"http://dev.celominds.com:8081/artifactory/celominds-supernet/dotnet-core/${env.JOB_NAME}-${env.BUILD_NUMBER}\""
		// 		}
		// 	}
		// }
		// stage ('Jfrog Artifactory: Upload') {
		// 	steps {
		// 		// rtUpload (
		// 		// 	serverId: "cs-artifactory",
		// 		// 	specPath: 'Artifactory/sql-upload-spec.json',
		// 		// 	failNoOp: true
		// 		// )
		// 		sh "curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -T Supernet.sql \"http://dev.celominds.com:8081/artifactory/celominds-supernet/database/${env.JOB_NAME}-${env.BUILD_NUMBER}\""
		// 	}
		// }
		// stage ('Jfrog Artifactory: Download') {
		// 	steps {
		// 		// rtDownload (
		// 		// 	serverId: "cs-artifactory",
		// 		// 	specPath: 'Artifactory/download-spec.json',
		// 		// 	failNoOp: true
		// 		// )
		// 		sh "cd /home/Artifactory/Celominds-Supernet | curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -O \"http://dev.celominds.com:8081/artifactory/celominds-supernet/database/${env.JOB_NAME}-${env.BUILD_NUMBER}/Supernet.sql\""
		// 		sh "cd /home/Artifactory/Celominds-Supernet | curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -O \"http://dev.celominds.com:8081/artifactory/celominds-supernet/dotnet-core/${env.JOB_NAME}-${env.BUILD_NUMBER}/dotnet-survey.tar.gz\""
		// 		sh "cd /home/Artifactory/Celominds-Supernet | tar -xvzf dotnet-supernet.tar.gz"
		// 	}
		// }
		// stage ('Archive Artifacts for Release') {
		// 	steps {
		// 		parallel Supernet: {
		// 			dir('Supernet/bin/Release') {
		// 				archiveArtifacts artifacts: 'netcoreapp2.0/**', fingerprint: true
		// 			}
		// 		},
		// 		ReadCSV: {
		// 			dir('GoLangProject/ReadCSV') {
		// 				archiveArtifacts artifacts: 'ReadCSVOutput/**', fingerprint: true
		// 			}
		// 		},
		// 		SupernetOutput: {
		// 			dir('GoLangProject/Supernet') {
		// 				archiveArtifacts artifacts: 'SupernetOutput/**', fingerprint: true
		// 			}
		// 		},
		// 		TargetJava: {
		// 			dir('Java/target') {
		// 				archiveArtifacts artifacts: '*jar', fingerprint: true
		// 			}
		// 		}
		// 	}
		// }
		// stage ('Docker: Clearing Running Containers') {
		// 	environment {
		// 		containerId = sh(script: "docker ps --quiet --filter name=sql-supernet dotnet-supernet", returnStdout: true).trim()
		// 	}
		// 	when {
		// 		expression {
		// 			return containerId.isEmpty()
		// 		}
		// 	}
		// 	steps {
		// 		steps {
		// 			sh "docker rm sql-supernet dotnet-supernet"
		// 		}
		// 	}
		// }
		// stage ('Deployment: SQL Server') {
		// 	steps {
		// 		sh "docker run --name sql-supernet -e \'ACCEPT_EULA=Y\' -e \'SA_PASSWORD=microIn@23\' -e \'MSSQL_PID=Express\' -p 8600:1433 -v /home/Artifactory/\"Celominds-Supernet\"/:/transfer -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu"
		// 		sh "docker exec -d sql-supernet /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \'microIn@23\' -Q \'CREATE DATABASE Supernet\'"
		// 		sh "docker exec -d sql-supernet /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \'microIn@23\' -Q -i /transfer/Supernet.sql"
		// 	}
		// }
		// stage ('Deployment: ASP.CORE Application') {
		// 	steps {
		// 		sh "docker run -d --name dotnet-supernet -v /home/Artifactory/\"Celominds-Supernet\"/:/transfer -p 8700:50620 --link sql-supernet:sql-supernet -d microsoft/dotnet"
		// 	}
		// }
	}
	post {
		success {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.SuccessJobCC}",
					message: JobSuccessSN
		}
		failure {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.FailureJobCC}",
					message: JobFailureSN
		}
		unstable {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.UnstableJobCC}",
					message: UnstableJobSN
		}
	}
}